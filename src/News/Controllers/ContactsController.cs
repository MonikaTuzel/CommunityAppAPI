﻿using Contacts.DTO;
using Contacts.IServices;
using Contacts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Contacts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private readonly IContactsService _contactsServices;
        public ContactsController(IContactsService contactsServices)
        {
            _contactsServices = contactsServices;
        }

        /// <summary>
        /// Pobieranie listy wszystkich adresów
        /// </summary>
        [HttpGet]
        public IEnumerable<Adress> BrowseAdresses()
        {
            return _contactsServices.BrowseAllAdress();
        }

        /// <summary>
        /// Pobieranie listy adresów po nazwie miasta
        /// </summary>
        [HttpGet("town/{townName}")]
        public IEnumerable<AdressDetailsDto> GetAdressDetails([FromRoute] string townName)
        {
            return _contactsServices.BrowseAdressByTown(townName);
        }

        /// <summary>
        /// Pobieranie listy adresów po nazwie województwa
        /// </summary>
        [HttpGet("district/{district}")]
        public IEnumerable<AdressDetailsDto> GetAdressFromDistrict([FromRoute]string district)
        {
            return _contactsServices.BrowseAdressByDistrict(district);
        }

        /// <summary>
        /// Tworzenie nowego adresu
        /// </summary>
        [HttpPost("[action]")]
        public void AddNewAdress([FromBody] CreateAdressDto createAdressDto)
        {
            _contactsServices.CreateAdress(createAdressDto);
        }

        /// <summary>
        /// Tworzenie nowego miasta
        /// </summary>
        [HttpPost("[action]")]
        public void AddNewTown([FromBody]CreateTownDto createTownDto)
        {
            _contactsServices.AddNewTown(createTownDto);
        }

        /// <summary>
        /// Uaktuanienie istniejącego adresu
        /// </summary>
        [HttpPut("[action]")]
        public void UpdateAdress([FromBody]UpdateAdressDto updateAdressDto, int townId)
        {
            _contactsServices.UpdateAdress(updateAdressDto, townId);
        }

        /// <summary>
        /// Usuwanie istniejącego adresu
        /// </summary>
        [HttpDelete("[action]")]
        public void DeleteAdress(int adressId)
        {
            _contactsServices.RemoveAdress(adressId);
        }
    }
}
