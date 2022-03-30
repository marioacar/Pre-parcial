using Contactos.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contactos.ViewModel
{
    public class ContactoViewModel : BaseViewModel
    {
        #region Propiedades
        public ObservableCollection<Contacto> Contactos { get; set; }
        #endregion

        private Contacto contacto;
        public Contacto Contacto
        {
            get { return contacto; }
            set { contacto = value; OnPropertyChanged(); }
        }

        public ICommand cmdContactoDetalle { get; set; }
        public ICommand cmdContactoElimina { get; set; }
        public ICommand cmdContactoEdita { get; set; }
        public ICommand cmdContactoCancelar { get; set; }
        public ICommand cmdContactoAgregar { get; set; }
        public ICommand cmdContactoAgregarTelefono { get; set; }
        public ICommand cmdContactoEliminarTelefono { get; set; }
        public ICommand cmdContactoGuardar { get; set; }
        public ContactoViewModel()
        {
            Contactos = new ObservableCollection<Contacto>();
            Contactos.Add(new Contacto()
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = "Mario",
                ApellidoPaterno = "Carvantes",
                ApellidoMaterno = "Gutierrez",
                Organizacion = "UdC",
                Telefonos = new ObservableCollection<Telefono> { new Telefono { Id = Guid.NewGuid().ToString(), Numero= "3125564851" },
                new Telefono {Id = Guid.NewGuid().ToString(), Numero= "312465654"} 
                }

            });

            Contactos.Add(new Contacto()
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = "AXL",
                ApellidoPaterno = "Bell",
                ApellidoMaterno = "Woods",
                Organizacion = "UdC",
                Telefonos = new ObservableCollection<Telefono> { new Telefono { Id = Guid.NewGuid().ToString(), Numero= "312584681" },
                new Telefono {Id = Guid.NewGuid().ToString(), Numero= "3125847550"}
                }

            });

            cmdContactoDetalle = new Command<Contacto>(async (x) => await PCmdContactoDetalle(x));

            cmdContactoElimina = new Command<Contacto>(async (x) => await PCmdContactoElimina(x));

            cmdContactoEdita = new Command<Contacto>(async (x) => await PCmdContactoEdita(x));

            cmdContactoCancelar = new Command(async () => await PCmdContactoCancelar());

            cmdContactoAgregar = new Command(async () => await PCmdContactoAgregar());

            cmdContactoEliminarTelefono = new Command<Telefono>(async (x) => await PCmdContactoEliminarTelefono(x));

            cmdContactoAgregarTelefono = new Command(async () => await PCmdContactoAgregarTelefono());

            cmdContactoGuardar = new Command<Contacto>(async (x) => await PCmdContactoGuardar(x));


            async Task PCmdContactoDetalle(Models.Contacto _Contacto)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new View.ContactoDetalle(_Contacto, this));
            }

            async Task PCmdContactoAgregar()
            {
                await Application.Current.MainPage.Navigation.PushAsync(new View.ContactoMatto(this));
            }

            async Task PCmdContactoAgregarTelefono()
            {
                if (Contacto.Telefonos == null)
                    Contacto.Telefonos = new ObservableCollection<Telefono>();
                Contacto.Telefonos.Add(new Telefono() { Id = Guid.NewGuid().ToString() });

                await Task.Delay(1000);
            }

            async Task PCmdContactoElimina(Models.Contacto _Contacto)
            {
                int indice = Contactos.IndexOf(_Contacto);

                if (indice >= 0)
                {
                    Contactos.Remove(_Contacto);
                    OnPropertyChanged();
                }

                await Application.Current.MainPage.Navigation.PopAsync();
            }

            async Task PCmdContactoEdita(Models.Contacto _Contacto)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new View.ContactoMatto(Contacto, this));
            }

            async Task PCmdContactoCancelar()
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }

            async Task PCmdContactoGuardar(Models.Contacto _Contacto)
            {
                int indice = -1;
                Contacto tmp = Contactos.FirstOrDefault(item => item.Id == _Contacto.Id);

                if (tmp != null)
                {
                    //Editando un contacto
                    indice = Contactos.IndexOf(tmp);
                    Contactos[indice] = _Contacto;
                }
                else
                {
                    Contactos.Add(_Contacto);
                }

                OnPropertyChanged();

                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PopAsync();
            }

            async Task PCmdContactoEliminarTelefono(Models.Telefono _Telefono)
            {
                Contacto.Telefonos.Remove(_Telefono);

                await Task.Delay(1000);

            }

        }
    }
}
