using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static TourneyMaster.MainWindow;
using static TourneyMaster.GenericCSharpHelper;

namespace TourneyMaster
{
    public class GenericXAMLHelper
    {
        /// <summary>
        /// Hides a TextBlock's text when a TextBox has text in it.
        /// </summary>
        /// <param name="textBoxName">The TextBox receiving inputs from the user.</param>
        /// <param name="textBlockName">The TextBlock wanting to be hidden.</param>
        public static void HideSearchText(TextBox textBoxName, TextBlock textBlockName)
        {
            textBlockName.Visibility = Visibility.Hidden;
            switch (textBoxName.Text.Length)
            {
                case 0:
                    textBlockName.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// Makes a TextBox refuse input if any item in a specific ListBox is selected.
        /// </summary>
        /// <param name="textBox">The TextBox you want to refuse input in.</param>
        /// <param name="listBox">The ListBox you want to monitor for selections.</param>
        public static void ToggleSearchBar(TextBox textBox, ListBox listBox)
        {
            switch (listBox.SelectedItems.Count)
            {
                case 0:
                    textBox.IsReadOnly = false;
                    break;
                default:
                    textBox.IsReadOnly = true;
                    break;
            }
        }

        /// <summary>
        /// Allows an easy date enter mechanic for TextBoxes. Should be used with PreviewKeyDown.
        /// </summary>
        /// <param name="textBox">The TextBox the date is being entered into</param>
        /// <param name="e">THe KeyEventArgs detected by the PreviewKeyDown</param>
        public static void EnterDate(TextBox textBox, KeyEventArgs e)
        {
            RestrictToNumbers(e);

            if(e.Handled == false)
            {
                string textEntered = "";
                if (textBox.Text != null)
                {
                    textEntered = textBox.Text;
                }

                string newText = "";

                if(e.Key == Key.Back && textEntered.Length > 0 && textEntered.Last() == '/')
                {
                    newText = textEntered.Remove(textEntered.Length - 2);
                }
                else if(e.Key == Key.Back && textEntered.Length > 0)
                {
                    newText = textEntered.Remove(textEntered.Length - 1);
                }
                else if(e.Key == Key.Back)
                {
                    newText = "";
                }
                else if(textEntered.Length == 10)
                {
                    newText = textEntered;
                }
                else if(textEntered.Length < 7 && textEntered.Length > 0 && textEntered.Last() != '/')
                {
                    newText = textEntered + e.Key.ToString().Last() + '/';
                }
                else if(textEntered.Length == 0)
                {
                    newText = e.Key.ToString().Last().ToString();
                }
                else if(textEntered.Length < 7 && textEntered.Length > 0 && textEntered.Last() == '/')
                {
                    newText = textEntered + e.Key.ToString().Last();
                }
                else if(textEntered.Length >= 7)
                {
                    newText = textEntered + e.Key.ToString().Last();
                }

                textBox.Text = newText;
                textBox.CaretIndex = textBox.Text.Count();

                e.Handled = true;
            }
        }

        /// <summary>
        /// Filters a ListBox's item contents based on a user's search term, while still preserving their selected items.
        /// </summary>
        /// <param name="listBox">The ListBox that will be filtered.</param>
        /// <param name="searchTerm">The search term the user inputs.</param>
        /// <param name="listBoxItemsRemoved">The running list of filtered list box items.</param>
        /// <param name="listBoxSelectedItemsRemoved">The running list of list box items that were filtered while they were still selected by the user.</param>
        public static void FilterListBoxItems<T>(ListBox listBox, string searchTerm, List<T> listBoxItemsRemoved, List<T> listBoxSelectedItemsRemoved)
        {
            //Grab the ListBox's currently selected items.
            List<T> listBoxItemsSelected = new List<T>(GetItemsFromListBox<T>(listBox, 2));

            //Add any items that were previously removed from the ListBox back in, then clear that list.
            AddItemsToListBox(listBox, listBoxItemsRemoved);
            listBoxItemsRemoved.Clear();

            //Grab the ListBox's items now that everything has been added back in.
            List<T> listBoxItems = new List<T>(GetItemsFromListBox<T>(listBox, 1));

            //Determine which items should be removed based on the search term, remove them, and add them to the listBoxItemsRemoved list.
            List<int> listBoxItemsToRemove = new List<int>(FilterItemList(listBoxItems, StringToList(searchTerm)));
            RemoveItemsFromListBox(listBox, listBoxItemsToRemove, listBoxItemsRemoved);

            //Sort the ListBox and set the user's selected items back if they are still an option.
            //SortListBoxItems<T>(listBox);
            SetListBoxSelectedItems(listBox, listBoxItemsSelected, listBoxItemsRemoved, listBoxSelectedItemsRemoved);
        }

        /// <summary>
        /// When an item in a ListBox is right clicked, this function figures out which one.
        /// </summary>
        /// <typeparam name="T">The type of items stored in the ListBox.</typeparam>
        /// <param name="listBox">The ListBox being right clicked.</param>
        /// <param name="listBoxSelectedBefore">The selected items that should have been recorded before the right click occurred.</param>
        /// <param name="listBoxItemRightClicked">The variable which contains the right clicked item.</param>
        /// <param name="debugLines">The relevant list of strings that the debugger will take for this function.</param>
        public static void ListBoxItemRightClickFunction<T>(ListBox listBox, List<T> listBoxSelectedBefore, ref T listBoxItemRightClicked, List<string> debugLines)
        {
            List<T> listBoxSelectedAfter = new List<T>(GetItemsFromListBox<T>(listBox, 2));
            listBoxItemRightClicked = GetRightClickedItem(listBoxSelectedBefore, listBoxSelectedAfter);

            if (listBoxSelectedBefore.IndexOf(listBoxItemRightClicked) == -1)
            {
                listBox.SelectedItems.Remove(listBoxItemRightClicked);
            }
            else
            {
                listBox.SelectedItems.Add(listBoxItemRightClicked);
            }
        }

        /// <summary>
        /// Decides which button should be showed based on whether or not there is text in a TextBox.
        /// </summary>
        /// <param name="amountTextBox">The TextBox you're monitoring.</param>
        /// <param name="removeButton">The button that will lose visibility if there is text.</param>
        /// <param name="changeButton">The button that will gain visibility if there is text.</param>
        public static void WhichButton(TextBox amountTextBox, Button removeButton, Button changeButton)
        {
            int amount = 0;
            int.TryParse(amountTextBox.Text, out amount);

            if (amountTextBox.Text.Length > 0 && amount != 0)
            {
                removeButton.Visibility = Visibility.Hidden;
                changeButton.Visibility = Visibility.Visible;
            }
            else
            {
                removeButton.Visibility = Visibility.Visible;
                changeButton.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Adds a list of listBoxItems to a ListBox.
        /// </summary>
        /// <param name="listBox">The ListBox you want to add the items to.</param>
        /// <param name="listBoxItems">The list of items you want to add.</param>
        public static void AddItemsToListBox<T>(ListBox listBox, List<T> listBoxItems)
        {
            foreach (T item in listBoxItems)
            {
                listBox.Items.Add(item);
            }
        }

        /// <summary>
        /// Removed a list of listBoxItems from a ListBox. Only use if you know for certain that the ListBox contains the list of items you want removed.
        /// </summary>
        /// <param name="listBox">The ListBox you want the items removed from.</param>
        /// <param name="listBoxItems">The items you want removed from the ListBox.</param>
        public static void RemoveListBoxItemsFromListBox<T>(ListBox listBox, List<T> listBoxItems)
        {
            foreach (T item in listBoxItems)
            {
                listBox.Items.Remove(item);
            }
        }

        /// <summary>
        /// Remove a list of items from a ListBox based on their index in the ListBox.
        /// </summary>
        /// <param name="listBox">The ListBox you are removing strings from.</param>
        /// <param name="listBoxItemsToRemove">The indexes of the items being removed.</param>
        /// <param name="listBoxItemsRemoved">A list of listBoxItems that have been removed from the ListBox.</param>
        public static void RemoveItemsFromListBox<T>(ListBox listBox, List<int> listBoxItemsToRemove, List<T> listBoxItemsRemoved)
        {
            List<T> listBoxItems = new List<T>(GetItemsFromListBox<T>(listBox, 1));
            for (int i = listBoxItemsToRemove.Count - 1; i >= 0; --i)
            {
                listBox.Items.RemoveAt(listBoxItemsToRemove[i]);
                listBoxItemsRemoved.Add(listBoxItems[listBoxItemsToRemove[i]]);
            }
        }

        /// <summary>
        /// Grabs the Items or SelecedItems from a ListBox and converts them into a list of listBoxItems.
        /// </summary>
        /// <param name="listBox">The ListBox you're grabbing from.</param>
        /// <param name="mode">1 = Grab Items. 2 = Grab SelectedItems.</param>
        /// <returns></returns>
        public static List<T> GetItemsFromListBox<T>(ListBox listBox, int mode)
        {
            List<T> items = new List<T>();
            switch (mode)
            {
                case 1:
                    items = listBox.Items.OfType<T>().ToList();
                    break;

                case 2:
                    items = listBox.SelectedItems.OfType<T>().ToList();
                    break;
            }

            return items;
        }

        /// <summary>
        /// Sets a ListBox's SelectedItems based on lists of selected items passed to it.
        /// </summary>
        /// <param name="listBox">The ListBox you want to set the selected items in.</param>
        /// <param name="listBoxSelected">A list of the ListBox's previously selected items.</param>
        /// <param name="listBoxItemsRemoved">The ListBox's list of items that have already been removed. Included here to be checked against in case the user filtered out a selected item.</param>
        /// <param name="listBoxSelectedItemsRemoved">If a ListBox has selected items removed, they are added to this list. This list is then checked against the current items to see if they are to be added back in.</param>
        public static void SetListBoxSelectedItems<T>(ListBox listBox, List<T> listBoxSelected, List<T> listBoxItemsRemoved, List<T> listBoxSelectedItemsRemoved)
        {
            //If selected items have been removed, check them against all the items in the ListBox at the moment. Add them back if they match.
            if (listBoxSelectedItemsRemoved != null)
            {
                for (int i = listBoxSelectedItemsRemoved.Count - 1; i >= 0; --i)
                {
                    if (listBox.Items.Contains(listBoxSelectedItemsRemoved[i]))
                    {
                        listBox.SelectedItems.Add(listBoxSelectedItemsRemoved[i]);
                        listBoxSelectedItemsRemoved.RemoveAt(i);
                    }
                }
            }

            //Runs through the list of previously selected items handed to the function.
            for (int i = 0; i < listBoxSelected.Count; ++i)
            {
                //If the ListBox currently has the item, it will select it.
                if (listBox.Items.Contains(listBoxSelected[i]))
                {
                    listBox.SelectedItems.Add(listBoxSelected[i]);
                }

                //Otherwise, it will check the removed items. If it finds it there, it will add the selected item to the list of selected items removed for future reference.
                else if (listBoxItemsRemoved != null && listBoxItemsRemoved.Contains(listBoxSelected[i]))
                {
                    listBoxSelectedItemsRemoved.Add(listBoxSelected[i]);
                }
            }
        }

        /// <summary>
        /// Restricts a TextBox's input to numbers, backspace, and enter. Must be used inside of a PreviewKeyUp/Down, not a regular KeyUp/Down.
        /// </summary>
        /// <param name="e">The TextBox's PreviewKeyUp/Down's KeyEventArgs parameter.</param>
        public static void RestrictToNumbers(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.Back:
                case Key.Enter:
                    e.Handled = false;
                    break;
                case Key.Space:
                    e.Handled = true;
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Disables the ability to right click items in a ListBox without removing any other functionality.
        /// </summary>
        /// <typeparam name="T">The type of item in the ListBox</typeparam>
        /// <param name="listBox">The ListBox you want to disable the right click functionality of</param>
        public static void DisableSelection<T>(ListBox listBox)
        {
            List<T> listBoxSelectedItems = new List<T>(GetItemsFromListBox<T>(listBox, 2));
            foreach (T item in listBoxSelectedItems)
            {
                listBox.SelectedItems.Remove(item);
            }
        }

        /// <summary>
        /// Identifies which item was right clicked in a ListBox based on the change of items present after the user right clicks.
        /// </summary>
        /// <typeparam name="T">The type of item in the ListBox</typeparam>
        /// <param name="itemsTrackedBefore">The ListBox's selected items before the right click</param>
        /// <param name="itemsTrackedAfter">The ListBox's selected items after the right click</param>
        /// <returns></returns>
        public static T GetRightClickedItem<T>(List<T> itemsTrackedBefore, List<T> itemsTrackedAfter)
        {
            T rightClickedItem = default;

            if (itemsTrackedBefore.Count < itemsTrackedAfter.Count)
            {
                foreach (T item in itemsTrackedAfter)
                {
                    if (itemsTrackedBefore.IndexOf(item) == -1)
                    {
                        rightClickedItem = item;
                    }
                }
            }
            else
            {
                foreach (T item in itemsTrackedBefore)
                {
                    if (itemsTrackedAfter.IndexOf(item) == -1)
                    {
                        rightClickedItem = item;
                    }
                }
            }

            return rightClickedItem;
        }

        /// <summary>
        /// Opens a folder select dialog box.
        /// </summary>
        /// <param name="path">The string you want to store the selected folder path in</param>
        public static void SelectFolder(ref string path)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                path = dialog.FileName;
            }
        }
    }
}
