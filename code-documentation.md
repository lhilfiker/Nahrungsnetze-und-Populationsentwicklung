# Code Documentation

# Nahrungsnetz Lists & Variables:

    -  List string "Names": The Name of the Animal/ Food
    -  List string "GetsEatenBy": From which animal it gets eaten(only one possible right now."" = eaten by nobody)
    -  List string "Eats": Eats the folloeing Animals Food(Ignored if FoodOrEater is true)
    -  List float "Quantity" : How many there are (For the simulation)
    -  List float "EatsHowMany" : How much food it needs per Day
    -  List bool "FoodOrEater" : true if its food. false if it is an animal

**Not Implemented yet:**
 - 


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



