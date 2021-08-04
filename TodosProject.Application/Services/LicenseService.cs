using AutoMapper;
using TodosProject.Application.Interfaces;
using TodosProject.Domain;
using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Enums;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using TodosProject.Infra.CrossCutting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity.SqlServer;

namespace TodosProject.Application.Services
{
    public class LicenseService : ILicenseService
    {
        public readonly IRepository<License> _licenceRepository;
        protected readonly IMapper _mapper;
        public LicenseService(IRepository<License> licenceRepository, IMapper mapper)
        {
            _licenceRepository = licenceRepository;
            _mapper = mapper;
        }

        public async Task<List<LicenseDto>> GetLicencesAvailableOrganization(long id)
        {
            return await _licenceRepository.FindBy(x => x.IsActive == true &&  x.Organization.Id == id)
                  .Select(x => new LicenseDto()
                  {
                      Id = x.Id.Value,
                      ActivationDate = x.ActivationDate,
                      ExpirationDate = x.ExpirationDate,
                      CodeID = x.CodeID,
                      MultiplayerMinutes = x.MultiplayerMinutes,
                      MaxFileCapacity = x.MaxFileCapacity

                  }).ToListAsync();
        }
        public ResultReturned Add(License li)
        {
            try
            {
                if (li.ActivationDate == DateTime.MinValue)
                    return new ResultReturned() { Result = false, Message = "Para realizar o cadastro, o campo ActivationDate deve ser preenchido" };

               
                    li.CreatedTime = DateTime.Now;
                    li.IsActive = true;

                    _licenceRepository.Add(li);
                    _licenceRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInAdd };
            }
            catch (Exception ex)
            {
                return new ResultReturned() { Result = false, Message = Constants.ErrorInAdd };
            }
        }

        public ResultReturned Update(long id, License li)
        {
            License liDb = _licenceRepository.GetById(id);

            if (liDb is not null)
            {
                liDb.ActivationDate = li.ActivationDate;
                liDb.ExpirationDate = li.ExpirationDate;
                liDb.MaxFileCapacity = li.MaxFileCapacity;
                liDb.MultiplayerMinutes = li.MultiplayerMinutes;
                liDb.IsActive = li.IsActive;
                liDb.Organization = li.Organization;
                liDb.UpdateTime = DateTime.Now;
                _licenceRepository.Update(liDb);
                _licenceRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInUpdate };
            }

            return new ResultReturned() { Result = true, Message = Constants.ErrorInUpdate };
        }

        public ResultReturned Delete(long id, bool isDeletePhysical = false)
        {
            License li = _licenceRepository.GetById(id);

            if (isDeletePhysical && li is not null)
                _licenceRepository.Remove(li);

            else if (!isDeletePhysical && li is not null)
            {
                li.UpdateTime = DateTime.Now;
                li.IsActive = li.IsActive ? false : true;
                _licenceRepository.Update(li);
                _licenceRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInDelete };
            }

            return new ResultReturned() { Result = false, Message = Constants.ErrorInDelete };
        }

        public UserPaged GetAllPaginate(LicenseFilter filter)
        {

            List<License> resultado = null;
            var query = _licenceRepository.GetAllTracking()
                   .Include(x => x.Organization)
                   .Where(x => (!filter.Status.HasValue || filter.Status.Value == x.IsActive)
                   && (!filter.IdOrganization.HasValue || (filter.IdOrganization.Value == x.IdOrganization))
                   && (string.IsNullOrEmpty(filter.CodeID) || x.CodeID.ToString().Trim().ToUpper().Contains(filter.CodeID.Trim().ToUpper()))
                   ).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                resultado = query.AsQueryable().AsNoTracking().ToList()
                    .Where(x =>
                    (x.ActivationDate == null || x.ActivationDate.ToString("dd/MM/yyyy").StartsWith(filter.Search.ToString())) ||
                    x.MultiplayerMinutes.ToString().Contains(filter.Search) ||
                    x.MaxFileCapacity.ToString().Contains(filter.Search) ||
                    (x.ExpirationDate != null ? x.ExpirationDate.Value.ToString("dd/MM/yyyy").Contains(filter.Search):true) ||
                    x.CodeID.ToString().ToLower().Contains(filter.Search.ToLower()) ||
                    x.Organization.Name.ToLower().Contains(filter.Search.ToLower())
                ).ToList();
            }
            else
            {
                resultado = query.AsQueryable().AsNoTracking().OrderByDescending(p => p.ActivationDate).ToList();
            }           
 
            var queryResult = _mapper.Map<List<LicenseDto>>(resultado);

            return new UserPaged
            {
                LicenseReturnedSet = queryResult.Count() > (filter.pageIndex) * filter.pageSize ? queryResult.Skip((filter.pageIndex) * filter.pageSize).Take(filter.pageSize).ToList() : queryResult.ToList(),
                NextPage = (filter.pageSize * filter.pageIndex) >= queryResult.Count() ? null : (int?)filter.pageIndex + 1,
                Page = filter.pageIndex,
                Total = queryResult.Count()
            };
        }

        public async Task<License> GetById(long id)
        {
            try
            {
                return await _licenceRepository.FindBy(x => x.Id == id)
                    .AsNoTracking()
                    .Include(x => x.Organization)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ResultReturned AddMinutesMultiplayer(string chaveLicense, long multiplayerMinutes)
        {
            try
            {
                License liDb = _licenceRepository.FindBy(x => x.CodeID.ToString() == chaveLicense).FirstOrDefault();

                if (liDb is not null)
                {
                    liDb.MultiplayerMinutes += multiplayerMinutes;
                    _licenceRepository.Update(liDb);
                    _licenceRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInUpdate };
                }
                return new ResultReturned() { Result = false, Message = Constants.ErrorInUpdate };
            }
            catch (Exception ex)
            {
                return new ResultReturned() { Result = false, Message = Constants.ErrorInUpdate };
            }
        }
        public List<License> GetAll()
        {
            return _licenceRepository.GetAll().ToList();
        }
    }
}
