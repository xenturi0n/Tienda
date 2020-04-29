using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Tienda.Common.Entidades;

namespace Tienda.Common.Validadores
{
    public class ProductosValidator : AbstractValidator<Productos>
    {
        public ProductosValidator()
        {
            RuleFor(p => p.Precio).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(p => p.Nombre).NotEmpty().NotNull().Length(1, 50);
        }
    }
}
