using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Tienda.Common.Entidades;

namespace Tienda.Common.Validadores
{
    public class VentasValidator : AbstractValidator<Ventas>
    {
        public VentasValidator()
        {
            RuleFor(v => v.FechaHora).NotNull().NotEmpty();
            RuleFor(v => v.Vendedor).NotNull().NotEmpty().Length(1, 50);
            RuleFor(v => v.Cliente).NotNull().NotEmpty().Length(1, 50);
        }
    }
}
