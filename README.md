1. Testability (show by example that you can cover your code with Unit and Integration tests, no need to cover all the lines)
All the logic of the project is abstracted by interfaces and it will be easily tested.

2. Extensibility (describe for what points/costs your code can be extended)
I divided project into separate small DLL layers, Presentation(API), Business, Persistence(Repository) and DataBase(LiteDb).

if we want to add any other logic (Service), we just add the classes like in CartService into different layers easily.
