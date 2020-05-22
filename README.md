# GooglePagination Intro
GooglePagination helps you to reduce boilerplate and setup pagination logic similar to what you see in Google search results.

The logic in Google's pagination is as follows:

there are 10 page links shown at any time (e.g. 1 2 3 4 5 6 7 8 9 10) unless there are less than 10 total pages
the active link (current page) is in the 6th position, except for when the active link is below 6 or less than 4 from the last position

Here's what it looks like for each page if there are 15 total pages:

- [1] 2 3 4 5 6 7 8 9 10
- 1 [2] 3 4 5 6 7 8 9 10
- 1 2 [3] 4 5 6 7 8 9 10
- 1 2 3 [4] 5 6 7 8 9 10
- 1 2 3 4 [5] 6 7 8 9 10
- 1 2 3 4 5 [6] 7 8 9 10
- 2 3 4 5 6 [7] 8 9 10 11
- 3 4 5 6 7 [8] 9 10 11 12
- 4 5 6 7 8 [9] 10 11 12 13
- 5 6 7 8 9 [10] 11 12 13 14
- 6 7 8 9 10 [11] 12 13 14 15
- 6 7 8 9 10 11 [12] 13 14 15
- 6 7 8 9 10 11 12 [13] 14 15
- 6 7 8 9 10 11 12 13 [14] 15
- 6 7 8 9 10 11 12 13 14 [15] 

While this looks pretty simple at first there's actually a bit of tricky logic to get it working correctly, particularly when the selected page is below 6 or less than 4 from the end, and also to cater for when there are more or less than 10 total pages.



Based On : https://jasonwatmore.com/post/2015/10/30/aspnet-mvc-pagination-example-with-logic-like-google

## Basic usage
Call DataPagingAsync() On Your IQueryable

```csharp
  IQueryable<Alarm> queryable = context.Alarms;
  
  var result = await queryable.DataPagingAsync(pageSize, currentPage);
  await result.DataList.TolistAsync();

```
The result is an instance of #PagingModel

```csharp
 public class PagingModel<T>
    {
        public IQueryable<T> DataList { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }

```

## Longer example 

```csharp
public async Task<PagingModel<T>> GetAlarmsAsync<T>(int pageSize = 15, int currentPage = 1, string filter = ""){
           
                if (currentPage < 1)
                    currentPage = 1;

                IQueryable<Alarm> alarms = GetAll().Include(a => a.Device).ThenInclude(a => a.Location);

                if (!string.IsNullOrEmpty(filter))
                {
                    alarms = alarms.Where(a => EF.Functions.Like(a.AlarmEvent.ToString(), $"%{filter}%") ||
                                            EF.Functions.Like(a.AlarmType.ToString(), $"%{filter}%") ||
                                            EF.Functions.Like(a.Device.Name, $"%{filter}%") ||
                                            EF.Functions.Like(a.Device.Alias, $"%{filter}%") ||
                                            EF.Functions.Like(a.Device.Location.Name, $"%{filter}%"));
                }

                var result = await alarms.DataPagingAsync(pageSize, currentPage);
                await result.DataList.Select(a => new AlarmViewModel
                {
                    Id = a.Id,
                    AlarmDate = a.AlarmDate,
                    AlarmEvent = a.AlarmEvent,
                    AlarmType = a.AlarmType,
                    DeviceName = a.Device.GetName(),
                    LocationName = a.Device.Location.Name,
                    TimeStamp = a.TimeStamp
                }).ToListAsync();
                
                return result;
}
```
