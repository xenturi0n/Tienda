using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Tienda.Common.Entidades;

namespace Tienda.Common.Validadores
{
    public class UsuariosValidator : AbstractValidator<Usuarios>
    {
        public UsuariosValidator()
        {
            RuleFor(u => u.Apellidos).NotNull().NotEmpty().Length(1, 50);
            RuleFor(u => u.NombreDeUsuario).NotNull().NotEmpty().Length(1, 50);
            RuleFor(u => u.Nombres).NotNull().NotEmpty().Length(1, 50);
            RuleFor(u => u.Password).NotNull().NotEmpty().Length(1, 50);

        }
    }
}
