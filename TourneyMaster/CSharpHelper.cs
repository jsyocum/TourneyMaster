using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Text;
using static System.String;
using static System.StringComparison;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static TourneyMaster.MainWindow;
using static TourneyMaster.GenericXAMLHelper;
using System.Windows.Input;

namespace TourneyMaster
{
    public class CSharpHelper
    {
        /// <summary>
        /// Changes a generic (T) variable/object's type.
        /// </summary>
        /// <typeparam name="T">The generic type T (Must be of type T).</typeparam>
        /// <typeparam name="U">The type you want it changed to.</typeparam>
        /// <param name="variable">The variable you want converted.</param>
        /// <returns></returns>
        public static U ChangeType<T, U>(T variable)
        {
            List<T> variableToList = new List<T>();
            variableToList.Add(variable);

            List<U> variableToUList = new List<U>(variableToList.OfType<U>());
            return variableToUList[0];
        }

        /// <summary>
        /// Takes a string and creates a list where that string is the only item.
        /// </summary>
        /// <param name="stringIn">The string you want to make into a list.</param>
        /// <returns></returns>
        public static List<string> StringToList(string stringIn)
        {
            List<string> stringListOut = new List<string>();
            stringListOut.Add(stringIn);

            return stringListOut;
        }

        /// <summary>
        /// Filter a list of items based on a list of strings.
        /// </summary>
        /// <param name="itemList">The list being filtered.</param>
        /// <param name="filter">The list to filter against.</param>
        /// <returns></returns>
        public static List<int> FilterItemList<T>(List<T> itemList, List<string> filter)
        {
            List<string> itemListToString = new List<string>();
            //if (typeof(T) == typeof(listBoxItem))
            //{
            //    List<listBoxItem> itemListAsListBoxItems = new List<listBoxItem>(itemList.OfType<listBoxItem>().ToList());

            //    foreach (listBoxItem item in itemListAsListBoxItems)
            //    {
            //        itemListToString.Add(item.ItemName);
            //    }
            //}
            //else if (typeof(T) == typeof(artefact))
            //{
            //    List<artefact> itemListAsArtefacts = new List<artefact>(itemList.OfType<artefact>().ToList());

            //    foreach (artefact arte in itemListAsArtefacts)
            //    {
            //        itemListToString.Add(arte.arteName);
            //    }
            //}

            List<string> itemListTarget = new List<string>(itemListToString);
            List<int> itemListItemsToRemove = new List<int>();
            //Run through every string in filter
            for (int j = 0; j < filter.Count; ++j)
            {
                //Run through every item in the target list, for each string in the filter
                for (int i = 0; i < itemListTarget.Count; ++i)
                {
                    //Compare every item's ItemName in the target list to the current string in the filter
                    switch (itemListTarget[i].Contains(filter[j], OrdinalIgnoreCase))
                    {
                        case false:
                            //If the substring is not found, run through every int in the list of strings to be removed
                            bool addedYet = false;
                            for (int k = 0; k < itemListItemsToRemove.Count; ++k)
                            {
                                //If this int has not been added to the list yet, add it
                                if (itemListItemsToRemove[k] == i)
                                {
                                    addedYet = true;
                                }
                            }

                            switch (addedYet)
                            {
                                case false:
                                    itemListItemsToRemove.Add(i);
                                    break;
                            }

                            //If the list of ints has not been given anything yet, this will start it
                            switch (itemListItemsToRemove.Count)
                            {
                                case 0:
                                    itemListItemsToRemove.Add(i);
                                    break;
                            }
                            break;
                    }
                }
            }

            return itemListItemsToRemove;
        }
    }
}