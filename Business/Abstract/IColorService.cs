﻿using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IColorService
    {
        IDataResult<List<Color>> GetAll();
        IDataResult<Color> GetById(int colorId);
        IResult Add(ColorAddDto colorAddDto);
        IResult Delete(Color color);
        IResult Update(ColorUpdateDto colorUpdateDto);        
    }
}
