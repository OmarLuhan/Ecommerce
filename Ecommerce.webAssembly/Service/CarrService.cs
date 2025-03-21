using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Ecommerce.webAssembly.Dto;

namespace Ecommerce.webAssembly.Service;
public interface ICarrService
{
   event Action OnChange;
   int CountItems();
   Task AddItemAsync(CarrDto item);
   Task RemoveItemAsync(CarrDto item);
   Task<IEnumerable<CarrDto>> GetItemsAsync();
   Task ClearAsync();
}
public class CarrService(ILocalStorageService localStorageService,
                        ISyncLocalStorageService syncLocalStorageService,
                        IToastService toastService):ICarrService
{
   public event Action? OnChange;
   public int CountItems()
   {
      var carrList = syncLocalStorageService.GetItem<List<CarrDto>>("carr");
      return carrList?.Count ?? 0;
   }
   public async Task AddItemAsync(CarrDto item)
   {
      try
      {
         var carrList = await localStorageService.GetItemAsync<List<CarrDto>>("carr") ?? [];
         var existingItem = carrList.FirstOrDefault(x => x.Product!.Id==item.Product!.Id);
         if (existingItem != null)
         {
            carrList.Remove(existingItem);
         }
         carrList.Add(item);
         await localStorageService.SetItemAsync("carr", carrList);
         toastService.ShowSuccess(existingItem != null ? "Item updated in carr" : "Item added to carr");
         OnChange?.Invoke();
      }catch(Exception ex)
      {
         toastService.ShowError(ex.Message);
      }
   }

   public async Task RemoveItemAsync(CarrDto item)
   {
      try
      {
         var carrList = await localStorageService.GetItemAsync<List<CarrDto>>("carr") ?? [];
         CarrDto? existingItem = null;
       if(carrList.Count!=0)
          existingItem = carrList.FirstOrDefault(x => x.Product!.Id==item.Product!.Id);
       
         if (existingItem != null)
         {
            carrList.Remove(existingItem);
            await localStorageService.SetItemAsync("carr", carrList);
            toastService.ShowSuccess("Item removed from carr");
            OnChange?.Invoke();
         }
      }catch(Exception ex)
      {
         toastService.ShowError(ex.Message);
      }
   }

   public async Task<IEnumerable<CarrDto>> GetItemsAsync()
   {
     var carrList = await localStorageService.GetItemAsync<List<CarrDto>>("carr");
     return carrList ?? [];
   }

   public async Task ClearAsync()
   {
     await localStorageService.RemoveItemAsync("carr");
       OnChange?.Invoke();
   }
}