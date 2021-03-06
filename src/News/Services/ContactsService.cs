using AutoMapper;
using Contacts.DataBase;
using Contacts.DTO;
using Contacts.Exceptions;
using Contacts.IServices;
using Contacts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contacts.Services
{
    public class ContactsService : IContactsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ContactsService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<Adress> BrowseAllAdress()
        {
            var adresses = _dbContext.Adress.ToList();
            return adresses;
        }       

        public IEnumerable<Town> BrowseAllTown()
        {
            return _dbContext.Town.ToList();
        }

        public IEnumerable<AdressDetailsDto> BrowseAllDetails()
        {
            var adresses = _dbContext.Adress
                .Include(x => x.Town)
                .Include(x => x.User)
                .ToList();

            if (!adresses.Any())
            {
                throw new NotFoundException($"Wystąpił błąd!");
            }

            var adressesDto = _mapper.Map<List<AdressDetailsDto>>(adresses);

            return adressesDto;
        }

        public async Task<AdressDetailsDto> GetAdressByUser(int userId)
        {
            var sdr = _dbContext.Adress.FirstOrDefault(x => x.UserId == userId);

            var adresses = _dbContext.Adress
                   .Include(x => x.Town)
                   .FirstOrDefault(c => c.UserId == userId);

            if (adresses == null)
            {
                throw new NotFoundException($"Wystąpił błąd!");
            }

            var adressesDto = _mapper.Map<AdressDetailsDto>(adresses);

            return adressesDto;

        }


        public IEnumerable<AdressDetailsDto> BrowseAdressByDistrict(string district)
        {
            var adresses = _dbContext.Adress
                .Include(x => x.Town)
                .Include(x => x.User)
                .Where(x => x.Town.District == district)
                .ToList();

            if (!adresses.Any())
            {
                throw new NotFoundException($"Brak adresów w powiecie: {district}!");
            }

            var adressesDto = _mapper.Map<List<AdressDetailsDto>>(adresses);

            return adressesDto;
        }

        public void CreateAdress(CreateAdressDto createAdressDto)
        {
            var townId = _dbContext.Town
                    .SingleOrDefault(x => x.Name == createAdressDto.TownName);
            var userId = _dbContext.User
                .SingleOrDefault(x => x.ShortName == createAdressDto.UserName);
          
            var newAdres = new Adress()
            {
                TownId = townId.Id,
                Code = createAdressDto.Code,
                Street = createAdressDto.Street,
                Number = createAdressDto.Number,
                UserId = userId.Id
            };

            _dbContext.Add(newAdres);
            _dbContext.SaveChanges();
        }

        public void AddNewTown(CreateTownDto createTownDto)
        {
            var newTown = new Town()
            {
                Name = createTownDto.Name,
                Province = createTownDto.Province,
                District = createTownDto.District,
                Commune = createTownDto.Commune
            };

            _dbContext.Town.Add(newTown);
            _dbContext.SaveChanges();
        }

        public void UpdateAdress(UpdateAdressDto updateAdressDto, int adressId)
        {
            var adress = _dbContext.Adress.SingleOrDefault(x => x.Id == adressId);
            if (adress is null)
            {
                throw new NotFiniteNumberException($"Podany adres nie istnieje!");
            }

            adress.Street = updateAdressDto.Street;
            adress.Number = updateAdressDto.Number;

            _dbContext.Adress.Update(adress);
            _dbContext.SaveChanges();
        }

        public void RemoveAdress(int adressId)
        {
            var adress = _dbContext.Adress.SingleOrDefault(x => x.Id == adressId);
            if (adress is null)
            {
                throw new NotFoundException($"Podany adres nie istnieje");
            }
            _dbContext.Adress.Remove(adress);
            _dbContext.SaveChanges();
        }

        
    }
}
