using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.applicationLayer.interfaces;
using apos_gestor_caja.Infrastructure.Repositories;

namespace apos_gestor_caja.ApplicationLayer.Services
{
    public class EmisorService : IEmisorService
    {
        private readonly EmisorRepository _emisorRepository;

        public EmisorService(EmisorRepository emisorRepository)
        {
            _emisorRepository = emisorRepository ?? throw new ArgumentNullException(nameof(emisorRepository));
            Console.WriteLine("EmisorService inicializado con EmisorRepository");
        }

        public async Task<bool> AddEmisorAsync(Emisor emisor)
        {
            if (emisor == null)
                throw new ArgumentNullException(nameof(emisor));
            if (string.IsNullOrWhiteSpace(emisor.Nombre))
                throw new ArgumentException("El nombre del emisor no puede estar vacío.", nameof(emisor.Nombre));

            Console.WriteLine($"Verificando si el emisor '{emisor.Nombre}' ya existe");
            if (await VerificarEmisorExistenteAsync(emisor.Nombre))
                throw new Exception($"Ya existe un emisor con el nombre '{emisor.Nombre}'.");

            Console.WriteLine($"Añadiendo emisor: {emisor.Nombre}");
            var resultado = await _emisorRepository.AddEmisorAsync(emisor);
            Console.WriteLine($"Emisor añadido {(resultado ? "exitosamente" : "sin éxito")}: {emisor.Nombre}");
            return resultado;
        }

        public async Task<List<Emisor>> ObtenerEmisoresAsync()
        {
            Console.WriteLine("Obteniendo lista de emisores");
            var emisores = await _emisorRepository.ObtenerEmisoresAsync();
            Console.WriteLine($"Emisores obtenidos - Total: {emisores.Count}");
            return emisores;
        }

        public async Task<Emisor> GetEmisorByIdAsync(int id)
        {
            if (id < 0)
                throw new ArgumentException("El ID del emisor no es válido.", nameof(id));

            Console.WriteLine($"Buscando emisor con ID: {id}");
            var emisor = await _emisorRepository.GetEmisorByIdAsync(id);
            if (emisor == null)
            {
                Console.WriteLine($"No se encontró emisor con ID: {id}");
                throw new Exception($"No se encontró el emisor con el ID {id}.");
            }

            Console.WriteLine($"Emisor encontrado - ID: {emisor.Id}, Nombre: {emisor.Nombre}");
            return emisor;
        }

        public async Task<bool> VerificarEmisorExistenteAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del emisor no puede estar vacío.", nameof(nombre));

            Console.WriteLine($"Verificando existencia del emisor: {nombre}");
            var existe = await _emisorRepository.VerificarEmisorExistenteAsync(nombre);
            Console.WriteLine($"Emisor '{nombre}' existe: {existe}");
            return existe;
        }

        public async Task<bool> ActualizarEmisorAsync(Emisor emisor)
        {
            if (emisor == null)
                throw new ArgumentNullException(nameof(emisor));
            if (string.IsNullOrWhiteSpace(emisor.Nombre))
                throw new ArgumentException("El nombre del emisor no puede estar vacío.", nameof(emisor.Nombre));
            if (emisor.Id < 0)
                throw new ArgumentException("El ID del emisor no es válido.", nameof(emisor.Id));

            Console.WriteLine($"Actualizando emisor con ID: {emisor.Id}, Nombre: {emisor.Nombre}");
            var resultado = await _emisorRepository.ActualizarEmisorAsync(emisor);
            if (!resultado)
            {
                Console.WriteLine($"No se encontró emisor con ID: {emisor.Id} para actualizar");
                throw new Exception($"No se pudo actualizar el emisor con ID {emisor.Id}. Puede que no exista.");
            }

            Console.WriteLine($"Emisor actualizado exitosamente - ID: {emisor.Id}");
            return resultado;
        }

        public async Task<Emisor> ActivarEmisorAsync(Emisor emisor)
        {
            if (emisor == null)
                throw new ArgumentNullException(nameof(emisor));
            if (emisor.Id < 0)
                throw new ArgumentException("El ID del emisor no es válido.", nameof(emisor.Id));

            Console.WriteLine($"Activando emisor con ID: {emisor.Id}");
            var emisorActualizado = await _emisorRepository.ActivarEmisorAsync(emisor);
            if (emisorActualizado == null)
            {
                Console.WriteLine($"No se encontró emisor con ID: {emisor.Id} para activar");
                throw new Exception($"No se encontró el emisor con el ID {emisor.Id} para activar.");
            }

            Console.WriteLine($"Emisor activado - ID: {emisorActualizado.Id}, Activo: {emisorActualizado.Activo}");
            return emisorActualizado;
        }

        public async Task<Emisor> DesactivarEmisorAsync(Emisor emisor)
        {
            if (emisor == null)
                throw new ArgumentNullException(nameof(emisor));
            if (emisor.Id < 0)
                throw new ArgumentException("El ID del emisor no es válido.", nameof(emisor.Id));

            Console.WriteLine($"Desactivando emisor con ID: {emisor.Id}");
            var emisorActualizado = await _emisorRepository.DesactivarEmisorAsync(emisor);
            if (emisorActualizado == null)
            {
                Console.WriteLine($"No se encontró emisor con ID: {emisor.Id} para desactivar");
                throw new Exception($"No se encontró el emisor con el ID {emisor.Id} para desactivar.");
            }

            Console.WriteLine($"Emisor desactivado - ID: {emisorActualizado.Id}, Activo: {emisorActualizado.Activo}");
            return emisorActualizado;
        }
    }
}