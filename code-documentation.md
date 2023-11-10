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
