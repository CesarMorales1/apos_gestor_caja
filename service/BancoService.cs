using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.applicationLayer.interfaces;
using apos_gestor_caja.Infrastructure.Repositories;

namespace apos_gestor_caja.ApplicationLayer.Services
{
    public class BancoService : IBancoService
    {
        private readonly BancoRepository _bancoRepository;

        public BancoService(BancoRepository bancoRepository)
        {
            _bancoRepository = bancoRepository ?? throw new ArgumentNullException(nameof(bancoRepository));
            Console.WriteLine("BancoService inicializado con BancoRepository");
        }

        public async Task<bool> AddBancoAsync(Banco banco)
        {
            if (banco == null)
                throw new ArgumentNullException(nameof(banco));
            if (string.IsNullOrWhiteSpace(banco.Nombre))
                throw new ArgumentException("El nombre del banco no puede estar vacío.", nameof(banco.Nombre));

            Console.WriteLine($"Verificando si el banco '{banco.Nombre}' ya existe");
            if (await VerificarBancoExistenteAsync(banco.Nombre))
                throw new Exception($"Ya existe un banco con el nombre '{banco.Nombre}'.");

            Console.WriteLine($"Añadiendo banco: {banco.Nombre}");
            var resultado = await _bancoRepository.AddBancoAsync(banco);
            Console.WriteLine($"Banco añadido {(resultado ? "exitosamente" : "sin éxito")}: {banco.Nombre}");
            return resultado;
        }

        public async Task<List<Banco>> ObtenerBancosAsync()
        {
            Console.WriteLine("Obteniendo lista de bancos");
            var bancos = await _bancoRepository.ObtenerBancosAsync();
            Console.WriteLine($"Bancos obtenidos - Total: {bancos.Count}");
            return bancos;
        }

        public async Task<Banco> GetBancoByIdAsync(int id)
        {
            if (id < 0)
                throw new ArgumentException("El ID del banco no es válido.", nameof(id));

            Console.WriteLine($"Buscando banco con ID: {id}");
            var banco = await _bancoRepository.GetBancoByIdAsync(id);
            if (banco == null)
            {
                Console.WriteLine($"No se encontró banco con ID: {id}");
                throw new Exception($"No se encontró el banco con el ID {id}.");
            }

            Console.WriteLine($"Banco encontrado - ID: {banco.Id}, Nombre: {banco.Nombre}");
            return banco;
        }

        public async Task<bool> VerificarBancoExistenteAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del banco no puede estar vacío.", nameof(nombre));

            Console.WriteLine($"Verificando existencia del banco: {nombre}");
            var existe = await _bancoRepository.VerificarBancoExistenteAsync(nombre);
            Console.WriteLine($"Banco '{nombre}' existe: {existe}");
            return existe;
        }

        public async Task<bool> ActualizarBancoAsync(Banco banco)
        {
            if (banco == null)
                throw new ArgumentNullException(nameof(banco));
            if (string.IsNullOrWhiteSpace(banco.Nombre))
                throw new ArgumentException("El nombre del banco no puede estar vacío.", nameof(banco.Nombre));
            if (banco.Id <= 0)
                throw new ArgumentException("El ID del banco no es válido.", nameof(banco.Id));

            Console.WriteLine($"Actualizando banco con ID: {banco.Id}, Nombre: {banco.Nombre}");
            var resultado = await _bancoRepository.ActualizarBancoAsync(banco);
            if (!resultado)
            {
                Console.WriteLine($"No se encontró banco con ID: {banco.Id} para actualizar");
                throw new Exception($"No se pudo actualizar el banco con ID {banco.Id}. Puede que no exista.");
            }

            Console.WriteLine($"Banco actualizado exitosamente - ID: {banco.Id}");
            return resultado;
        }

        public async Task<Banco> ActivarBancoAsync(Banco banco)
        {
            if (banco == null)
                throw new ArgumentNullException(nameof(banco));
            if (banco.Id <= 0)
                throw new ArgumentException("El ID del banco no es válido.", nameof(banco.Id));

            Console.WriteLine($"Activando banco con ID: {banco.Id}");
            var bancoActualizado = await _bancoRepository.ActivarBancoAsync(banco);
            if (bancoActualizado == null)
            {
                Console.WriteLine($"No se encontró banco con ID: {banco.Id} para activar");
                throw new Exception($"No se encontró el banco con el ID {banco.Id} para activar.");
            }

            Console.WriteLine($"Banco activado - ID: {bancoActualizado.Id}, Activo: {bancoActualizado.Activo}");
            return bancoActualizado;
        }

        public async Task<Banco> DesactivarBancoAsync(Banco banco)
        {
            if (banco == null)
                throw new ArgumentNullException(nameof(banco));
            if (banco.Id <= 0)
                throw new ArgumentException("El ID del banco no es válido.", nameof(banco.Id));

            Console.WriteLine($"Desactivando banco con ID: {banco.Id}");
            var bancoActualizado = await _bancoRepository.DesactivarBancoAsync(banco);
            if (bancoActualizado == null)
            {
                Console.WriteLine($"No se encontró banco con ID: {banco.Id} para desactivar");
                throw new Exception($"No se encontró el banco con el ID {banco.Id} para desactivar.");
            }

            Console.WriteLine($"Banco desactivado - ID: {bancoActualizado.Id}, Activo: {bancoActualizado.Activo}");
            return bancoActualizado;
        }
    }
}