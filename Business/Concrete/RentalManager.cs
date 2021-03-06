﻿using AutoMapper;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        private readonly IRentalDal _rentalDal;
        private readonly IMapper _mapper;

        public RentalManager(IRentalDal rentalDal, IMapper mapper)
        {
            _rentalDal = rentalDal;
            _mapper = mapper;
        }

        #region Listeleme Metotları
        [SecuredOperation("Admin")]
        [LogAspect(typeof(FileLogger))]
        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll());
        }
        [SecuredOperation("Admin")]
        [LogAspect(typeof(FileLogger))]
        public IDataResult<Rental> GetById(int rentalId)
        {
            return new SuccessDataResult<Rental>(_rentalDal.Get(p => p.Id == rentalId));
        }

        #endregion

        #region Temel Ekleme-Silme-Güncelleme
        [SecuredOperation("Admin")]
        [LogAspect(typeof(FileLogger))]
        public IResult Add(RentalAddDto rentalAddDto)
        {
            //Business rules
            IResult result = BusinessRules.Run(CheckIfCarIsRented(rentalAddDto));
            if (result!=null)
            {
                return result;
            }
            // mapper
            Rental rental = _mapper.Map<Rental>(rentalAddDto);
            _rentalDal.Add(rental);
            return new SuccessResult(Messages.RentalAdded);
        }
        [SecuredOperation("Admin")]
        [LogAspect(typeof(FileLogger))]
        public IResult Delete(Rental rental)
        {
            _rentalDal.Delete(rental);
            return new SuccessResult(Messages.RentalDeleted);

        }
        [SecuredOperation("Admin")]
        [LogAspect(typeof(FileLogger))]
        public IResult Update(RentalUpdateDto rentalUpdateDto)
        {
            //mapper
            Rental rental = _mapper.Map<Rental>(rentalUpdateDto);
            _rentalDal.Update(rental);
            return new SuccessResult(Messages.RentalUpdated);
        }

        #endregion


        #region Business Rules

        private IResult CheckIfCarIsRented(RentalAddDto rentalAddDto)
        {
            // İlgili aracın dönüş tarihi varsa - Araç kiralanmış
            var result = _rentalDal.GetAll(p => p.CarId == rentalAddDto.CarId && p.ReturnDate != null);
            if (result.Count > 0)
            {
                return new ErrorResult(Messages.RentalReturnDateError);
            }
            return new SuccessResult();
        }
    }

    #endregion

}
