# Code Documentation


## database.cs
Class Database

For what:
- Read and return The Save File with `Database.OpenDatabase(the path of the file)`, it returns:
    -  List string "Names"
    -  List string "GetsEatenBy"
    -  List string "Eats"
    -  List float "Quantity" 
    -  List float "EatsHowMany" 
    -  List bool "FoodOrEater" 

- Save to the Database File `Database.SaveToDatabase(all the lists listed down)`, this includes, in this order:
    -  List string "Names"
    -  List string "GetsEatenBy"
    -  List string "Eats"
    -  List float "Quantity" 
    -  List float "EatsHowMany" 
    -  List bool "FoodOrEater" 
    - filepath of the file to save to



## operation-helper.cs
Class OperationHelper

Functions:
- Sort By Layer `OperationHelper.SortByLayer(all the lists listed down)`, this includes, in this order:
    -  List string "Names"
    -  List string "GetsEatenBy"
    -  List string "Eats"
    -  List float "Quantity" 
    -  List float "EatsHowMany" 
    -  List bool "FoodOrEater" 

    **It will return the 2 Lists<int>**
    - List 1: All Indexes of the input sorted by layer. e.g
      | Indexes | Vaue |
      |-----------------|-----------------|
      | 0 | 15 |
      | 1 | 12 |
      | 2 | 5 |
      | 3 | 8 |
      | 4 | 11 |
      | 5 | 7 |
      | 6 | 3 |
      | 7 | 1 |
      | 8 | 2 |
    - List 2: Where the next layer stops:
      | Indexes | Vaue |
      |-----------------|-----------------|
      | 0 | 2 |
      | 1 | 5 |
      | 2 | 6 |
      | 3 | 9 |

    - The First Layer in this example goes to index 2 of the first List(index 0 of List2), The Second Layer goes to Index 5 of the first Layer, Layer 3 goes to Index 6 and Layer 4 to Index 9.



