using Fiap.Web.AspNet.Controllers.Filters;
using Fiap.Web.AspNet.Models;
using Fiap.Web.AspNet.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fiap.Web.AspNet.Controllers
{
    public class ClienteController : Controller
    {

        private ClienteRepository clienteRepository;
        private RepresentanteRepository representanteRepository;

        public ClienteController()
        {

            clienteRepository = new ClienteRepository();    
            representanteRepository = new RepresentanteRepository();
        }

        [LogFilter]
        public IActionResult Index()
        {
            // Retornando para View a lista de Clientes
            var lista = clienteRepository.Listar();

            return View(lista); 
        }

        // Anotação de uso do Verb HTTP Get
        [HttpGet]
        public IActionResult Cadastrar()
        {
            // Método que carrega os dados do representante e
            // monta uma combo para exibição na tela
            ComboRepresentantes();

            return View(new ClienteModel());
        }

        // Anotação de uso do Verb HTTP Post
        [HttpPost]
        public IActionResult Cadastrar(ClienteModel cliente)
        {
            if ( ModelState.IsValid ) {

                clienteRepository.Inserir(cliente);

                TempData["mensagem"] = "Cliente cadastrado com sucesso";
                return RedirectToAction("Index", "Cliente");
            } else
            {
                // Método que carrega os dados do representante e
                // monta uma combo para exibição na tela
                ComboRepresentantes();

                return View(cliente);
            }
        }


        [HttpGet]
        public IActionResult Editar([FromRoute] int id)
        {
            // Método que carrega os dados do representante e
            // monta uma combo para exibição na tela
            ComboRepresentantes(); 

            var Cliente = clienteRepository.Consultar(id);
            return View(Cliente);
        }

        [HttpPost]
        public IActionResult Editar(ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                clienteRepository.Alterar(cliente);

                TempData["mensagem"] = "Cliente alterado com sucesso";
                return RedirectToAction("Index", "Cliente");
            }
            else
            {
                // Método que carrega os dados do representante e
                // monta uma combo para exibição na tela
                ComboRepresentantes();

                return View(cliente);
            }

        }


        [HttpGet]
        public IActionResult Consultar(int id)
        {
            var Cliente = clienteRepository.Consultar(id);
            return View(Cliente);
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            clienteRepository.Excluir(id);

            TempData["mensagem"] = "Cliente excluído com sucesso";
            return RedirectToAction("Index", "Cliente");
        }


        private void ComboRepresentantes()
        {
            var listaRepresentantes = representanteRepository.Listar();
            var selectListRepresentantes = new SelectList(listaRepresentantes, "RepresentanteId", "NomeRepresentante");
            ViewBag.representantes = selectListRepresentantes;
        }

    }
}
