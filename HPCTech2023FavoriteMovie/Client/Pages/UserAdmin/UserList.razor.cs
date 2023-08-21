using HPCTech2023FavoriteMovie.Client.HttpRepository;
using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;

namespace HPCTech2023FavoriteMovie.Client.Pages.UserAdmin;

public partial class UserList
{
    private List<UserDto> users = null;
    [Inject]
    public IUserMoviesHttpRepository UserMovieHttpRepository { get; set; }
    public SfGrid<UserDto> userGrid { get; set; }
    public List<string> roles { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // add try and catch to show user with toast
        var res = await UserMovieHttpRepository.GetUsers();
        if (res.Succeeded)
        {
            users = res.Data;
        }
    }

    private async Task GetUserRoles(RowSelectEventArgs<UserDto> args)
    {
        // add try catch with toast to show user error
        var res = await UserMovieHttpRepository.GetRoles(args.Data.Id);
        roles = res.Data;
    }
}
