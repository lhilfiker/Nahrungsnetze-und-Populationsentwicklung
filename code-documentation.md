# Code Documentation

## Food Web Lists & Variables

### Variables Description
- **Names** (`List<string>`): Stores the names of animals or food items.
- **GetsEatenBy** (`List<string>`): Indicates the predator for each item in `Names`. An empty string signifies no predator. Limited to one item per animal currently.
- **Eats** (`List<string>`): Lists the prey or food each animal consumes. Limited to one item per animal currently.
- **Quantity** (`List<float>`): Represents the population or quantity of each item in `Names`.
- **EatsHowMany** (`List<float>`): The amount of food each animal requires per day.
- **FoodOrEater** (`List<bool>`): Flags whether an item is food (`true`) or an animal (`false`).

### Future Implementations
- 

## Database.cs

### Class: Database
This class is responsible for handling data persistence, including reading and saving the state of the food web.

#### Methods
- **OpenDatabase** `(string filePath)`: Reads the save file located at `filePath`. Returns all the above lists.
- **SaveToDatabase** `(List<string> Names, List<string> GetsEatenBy, List<string> Eats, List<float> Quantity, List<float> EatsHowMany, List<bool> FoodOrEater, string filePath)`: Saves the current state to a file located at `filePath`.

## OperationHelper.cs

### Class: OperationHelper
This class contains functions to perform operations like sorting the food web layers.

#### Methods
- Function: SortByLayer
This function organizes the food web elements into hierarchical layers based on their trophic levels (i.e., their position in the food chain).

   Input Parameters
   - Lists of `Names`, `GetsEatenBy`, `Eats`, `Quantity`, `EatsHowMany`, and `FoodOrEater`.

   Output
   - **LayerIndexes** (`List<int>`): A list of indexes representing the sorted order of elements as per their trophic layers.
   - **LayerBoundaries** (`List<int>`): A list indicating the end index within `LayerIndexes` for each trophic layer.

   Example
   Suppose we have the following input data:

   - **Names**: [Plant, Rabbit, Fox, Grasshopper, Snake]
   - **GetsEatenBy**: [Rabbit, Fox, "", Grasshopper, Fox]
   - **Eats**: ["", Plant, Rabbit, Plant, Grasshopper]

   After applying `SortByLayer`, we might get the following output:

   - **LayerIndexes**:
     | Index | Value |
     |-------|-------|
     | 0     | 3     | (Grasshopper)
     | 1     | 0     | (Plant)
     | 2     | 1     | (Rabbit)
     | 3     | 4     | (Snake)
     | 4     | 2     | (Fox)

   - **LayerBoundaries**:
     | Index | Value |
     |-------|-------|
     | 0     | 2     | (End of Producers: Plant, Grasshopper)
     | 1     | 4     | (End of Primary Consumers: Rabbit, Snake)
     | 2     | 5     | (End of Secondary Consumers: Fox)

   Explanation
   - **LayerIndexes**: This shows that Grasshopper (index 3) and Plant (index 0) are at the bottom of the food chain (producers). Rabbit (index 1) and Snake (index 4) are the next level (primary consumers), and Fox (index 2) is a secondary consumer.
   - **LayerBoundaries**: Indicates the division of trophic levels. The first two elements (indices 0 and 3) are producers, the next two (indices 1 and 4) are primary consumers, and the last one (index 2) is a secondary consumer.

### Version Information
- Last Updated: 11.11.2023

### Author
- RebelCoderJames


