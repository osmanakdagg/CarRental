﻿using AutoMapper;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IMapper _mapper;

        public UserManager(IUserDal userDal, IMapper mapper)
        {
            _userDal = userDal;
            _mapper = mapper;
        }

        #region Listeleme Metotları

        [SecuredOperation("Admin")]
        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll());
        }
        [SecuredOperation("Admin")]
        public IDataResult<User> GetById(int userId)
        {
            return new SuccessDataResult<User>(_userDal.Get(p => p.Id == userId));   
        }
        [SecuredOperation("Admin")]
        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>> (_userDal.GetClaims(user));
        }
        [SecuredOperation("Admin")]
        public IDataResult<User> GetByMail(string email)
        {
            return new SuccessDataResult<User>(_userDal.Get(p=>p.Email==email));
        }

        #endregion

        #region Temel Ekleme-Silme-Güncelleme
        [SecuredOperation("Admin")]
        public IResult Add(UserAddDto userAddDto)   
        {
            //mapping
            User user = _mapper.Map<User>(userAddDto);
            _userDal.Add(user);
            return new SuccessResult(Messages.UserAdded);
        }
        [SecuredOperation("Admin")]
        public IResult Delete(User user)
        {
            _userDal.Delete(user);
            return new SuccessResult(Messages.UserDeleted);
        }
        [SecuredOperation("Admin")]
        public IResult Update(UserUpdateDto userUpdateDto)
        {
            //mapping
           User user = _mapper.Map<User>(userUpdateDto);
            _userDal.Update(user);
            return new SuccessResult(Messages.UserUpdated);
        }

       
        #endregion


    }
}
