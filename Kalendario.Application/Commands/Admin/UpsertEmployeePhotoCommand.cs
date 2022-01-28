using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Authorization;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Employee), Employee.UpdateRole)]
public class UpsertEmployeePhotoCommand : IKalendarioProtectedCommand<EmployeeAdminResourceModel>
{
    public Guid Id { get; set; }
    
    public Stream Image { get; set; }

    public class Handler : IRequestHandler<UpsertEmployeePhotoCommand, EmployeeAdminResourceModel>
    {
        private readonly IKalendarioDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserManager _currentUserManager;
        private readonly IImageUploaderService _imageUploaderService;

        public Handler(IKalendarioDbContext context,
            IMapper mapper,
            ICurrentUserManager currentUserManager,
            IImageUploaderService imageUploaderService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserManager = currentUserManager;
            _imageUploaderService = imageUploaderService;
        }

        public async Task<EmployeeAdminResourceModel> Handle(UpsertEmployeePhotoCommand request, CancellationToken cancellationToken)
        {
            var domain = await _context.Employees
                .Where(e => e.AccountId == _currentUserManager.CurrentUserAccountId)
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            
            if (domain == default)
            {
                throw new NotFoundException(nameof(Employee), request.Id);
            }

            var photoUrl = await _imageUploaderService.UploadImage(domain.Id.ToString(), request.Image);

            domain.PhotoUrl = photoUrl;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EmployeeAdminResourceModel>(domain);
        }
    }
}