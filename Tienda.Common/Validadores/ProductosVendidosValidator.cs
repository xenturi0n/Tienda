using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Tienda.Common.Entidades;

namespace Tienda.Common.Validadores
{
    public class ProductosVendidosValidator : AbstractValidator<ProductosVendidos>
    {
        public ProductosVendidosValidator()
        {
            RuleFor(p => p.Cantidad).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(p => p.PrecioDeVenta).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(p => p.IdProducto).NotNull().NotEmpty();
            RuleFor(p => p.IdVenta).NotNull().NotEmpty();
        }
    }
}