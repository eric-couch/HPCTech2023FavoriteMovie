using HPCTech2023FavoriteMovie.Client.HttpRepository;
using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using static System.Net.WebRequestMethods;
using Syncfusion.Blazor.Notifications.Internal;
using Syncfusion.Blazor.Navigations;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;

namespace HPCTech2023FavoriteMovie.Client.Pages.UserAdmin;

public partial class UserList
{
    private List<UserDto> users = null;
    [Inject]
    public IUserMoviesHttpRepository UserMovieHttpRepository { get; set; }
    public SfGrid<UserDto> userGrid { get; set; }
    public SfGrid<RoleDto> userRoleGrid { get; set; }
    public List<RoleDto> roles { get; set; }
    public string selectedRole { get; set; } = String.Empty;
    public bool IsAddRoleVisible { get; set; } = false;
    public bool IsAddUserToRoleVisible { get; set; } = false;
    public RoleDto newRole { get; set; } = new(); 
    

    protected override async Task OnInitializedAsync()
    {
        // add try and catch to show user with toast
        var res = await UserMovieHttpRepository.GetUsers();
        if (res.Succeeded)
        {
            users = res.Data;
        }
    }

    private async Task GetRole(RowSelectEventArgs<RoleDto> args)
    {
        selectedRole = args.Data.Name;
    }

    //ToolbarClickEventHandler
    public async Task ToolbarClickEventHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "GridRoleAdd")
        {
            IsAddRoleVisible = true;
        }
    }

    public async Task AddNewRole(MouseEventArgs args)
    {
        IsAddRoleVisible = true;

    }

    public async void AddRole(EditContext ec)
    {
        // call add role repo method
        var res = await UserMovieHttpRepository.AddRole(newRole.Name);
        if (res.Succeeded)
        {
            // add toast
            
        } else
        {
            // add toast
            
        }
        IsAddRoleVisible = false;
        StateHasChanged();
    }

    private async Task GetUserRoles(RowSelectEventArgs<UserDto> args)
    {
        // add try catch with toast to show user error
        var res = await UserMovieHttpRepository.GetRoles(args.Data.Id);
        roles = res.Data;
    }
}
