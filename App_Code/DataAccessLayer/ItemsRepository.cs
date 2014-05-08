using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// 
/// </summary>
public class ItemsRepository
{
    //public static List<Item> Items = null;
    public static ConcurrentDictionary<int, Item> Items = null;

    /// <summary>
    /// Create a collection of items, 4 to be precisely
    /// </summary>
    public static void create()
    {
        int initialCapacity = 4;

        // The higher the concurrencyLevel, the higher the theoretical number of operations 
        // that could be performed concurrently on the ConcurrentDictionary.  However, global 
        // operations like resizing the dictionary take longer as the concurrencyLevel rises.  
        // For the purposes of this example, we'll compromise at numCores * 2. 
        int numProcs = Environment.ProcessorCount;
        int concurrencyLevel = numProcs * 2;

        // Construct the dictionary with the desired concurrencyLevel and initialCapacity
        Items = new ConcurrentDictionary<int, Item>(concurrencyLevel, initialCapacity);
        Items.TryAdd(0, new Item { Order = 0, Description = "Description of the first Item", Title = "first", Image = "img01.jpg" });
        Items.TryAdd(1, new Item { Order = 1, Description = "Description of the second Item", Title = "second", Image = "img02.jpg" });
        Items.TryAdd(2, new Item { Order = 2, Description = "Description of the third Item", Title = "third", Image = "img03.jpg" });
        Items.TryAdd(3, new Item { Order = 3, Description = "Description of the fourth Item", Title = "fourth", Image = "img04.jpg" });

        //An enumerator remains valid as long as the collection remains unchanged. 
        //If changes are made to the collection, such as adding, modifying, or deleting elements, 
        //the enumerator is irrecoverably invalidated and its behavior is undefined.
        //The enumerator does not have exclusive access to the collection; therefore, 
        //enumerating through a collection is intrinsically not a thread-safe procedure.
        //Items = new List<Item>{  new Item { Order =0, Description ="Description of the first Item", Title ="first", Image ="img01.jpg"},
        //                         new Item { Order =1, Description ="Description of the second Item", Title ="second", Image ="img02.jpg"},
        //                         new Item { Order =2, Description ="Description of the third Item", Title ="third", Image ="img03.jpg"},
        //                         new Item { Order =3, Description ="Description of the fourth Item", Title ="fourth", Image ="img04.jpg"}};
    }

    /// <summary>
    /// Get an item by the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns></returns>
    //public static Item getByOrder(byte order)
    //{
    //    return (from p in Items where p.Order == order select p).FirstOrDefault();
    //}

    public static Item getByOrder(byte order)
    {
        Item it = null;
        Items.TryGetValue(order, out it);
        return it;
    }

    ///// <summary>
    ///// Adds the specified Item at index.
    ///// </summary>
    ///// <param name="index">The index.</param>
    ///// <param name="item">The item.</param>
    ///// <returns></returns>
    //public static Item insert(byte index, Item item)
    //{
    //    try
    //    {
    //        Items.RemoveAt(index); //remove first
    //        Items.Insert(index, item); // then add an item
    //        return item;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    public static Item insert(byte index, Item item)
    {
        if (Items.TryUpdate(index, item, getByOrder(index)))
            return item;
        else
            return null;
    }

    ///// <summary>
    ///// Removes the specified Item at index.
    ///// </summary>
    ///// <param name="index">The index.</param>
    //public static void remove(int index)
    //{
    //    Items.RemoveAt(index);
    //}

    ///// <summary>
    ///// Adds the specified item.
    ///// </summary>
    ///// <param name="item">The item.</param>
    //public static void add(Item item)
    //{
    //    Items.Add(item);
    //}


}