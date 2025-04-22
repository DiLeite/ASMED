using ASMED.Data.Context;
using ASMED.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ASMED.API.Utils;

public class SurgeryDocumentImporter
{
    private readonly AsmedContext _context;
    private readonly string _anexosPath;

    public SurgeryDocumentImporter(AsmedContext context, string anexosPath)
    {
        _context = context;
        _anexosPath = anexosPath;
    }

    public async Task ImportAsync()
    {
        var pastaRaiz = new DirectoryInfo(_anexosPath);
        if (!pastaRaiz.Exists)
        {
            Console.WriteLine("Pasta de anexos não encontrada.");
            return;
        }

        foreach (var pastaPaciente in pastaRaiz.GetDirectories())
        {
            var nome = pastaPaciente.Name;
            var nomeSemId = string.Join(" ", nome.Split('_').TakeWhile(s => !long.TryParse(s, out _))).Trim();

            if (string.IsNullOrEmpty(nomeSemId)) continue;

            var paciente = await _context.Patients.FirstOrDefaultAsync(p =>
                EF.Functions.Like(p.FullName, $"%{nomeSemId.Replace("_", " ")}%"));

            if (paciente == null)
            {
                Console.WriteLine($"Paciente '{nomeSemId}' não encontrado no banco.");
                continue;
            }

            var cirurgia = await _context.Surgeries
                .Where(s => s.PatientId == paciente.Id)
                .OrderByDescending(s => s.ScheduledDateTime)
                .FirstOrDefaultAsync();

            if (cirurgia == null)
            {
                Console.WriteLine($"Nenhuma cirurgia encontrada para o paciente '{paciente.FullName}'.");
                continue;
            }

            foreach (var file in pastaPaciente.GetFiles())
            {
                var tipo = file.Extension.TrimStart('.').ToLower();
                var caminhoRelativo = Path.Combine("Anexos", pastaPaciente.Name, file.Name);

                var doc = new SurgeryDocument
                {
                    Id = Guid.NewGuid(),
                    SurgeryId = cirurgia.Id,
                    FileUrl = caminhoRelativo,
                    Type = tipo,
                    UploadedAt = DateTime.UtcNow
                };

                _context.SurgeryDocuments.Add(doc);
                Console.WriteLine($"Documento '{file.Name}' vinculado à cirurgia do paciente {paciente.FullName}");
            }
        }

        await _context.SaveChangesAsync();
        Console.WriteLine("Importação finalizada.");
    }
}
