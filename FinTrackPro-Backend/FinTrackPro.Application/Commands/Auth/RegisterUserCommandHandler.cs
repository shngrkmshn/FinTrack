using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Entities;
using MediatR;

namespace FinTrackPro.Application.Commands.Auth;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthTokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        ICategoryRepository categoryRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthTokenDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var emailAlreadyExists = await _userRepository.EmailExistsAsync(command.Email, cancellationToken);

        if (emailAlreadyExists)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = User.Create(command.Email, passwordHash, command.FirstName, command.LastName);

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        var uncategorized = Category.Create("Uncategorized", user.Id);
        await _categoryRepository.AddAsync(uncategorized, cancellationToken);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        return new AuthTokenDto(accessToken, refreshToken, _jwtTokenService.AccessTokenExpirationMinutes);
    }
}
