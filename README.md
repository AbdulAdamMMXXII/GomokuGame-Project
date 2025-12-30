# Gomoku Game – F# Functional Programming Project

A command‑line Gomoku (Five‑in‑a‑Row) game built using **F#** and the **declarative programming paradigm**. The project demonstrates functional design, immutability, recursion, pattern matching, and behaviour‑driven development using **Gherkin**, **Hoare Logic**, **xUnit**, **FsUnit**, and **TickSpec**.

---

## 🎮 Features

- 15×15 Gomoku board  
- Two‑player mode  
- Single‑player mode vs CPU  
- Turn‑based gameplay with player switching  
- Win detection (horizontal, vertical, diagonal)  
- Draw detection  
- Input validation and error handling  
- Clear CLI board rendering  
- Modular architecture for maintainability  

---

## 🧠 Functional Programming Concepts Used

- **Immutability** for predictable state transitions  
- **Pattern matching** for game logic and board evaluation  
- **Recursion** for game loops and win‑checking traversal  
- **Discriminated unions** for Player, Cell, and GameMode types  
- **Higher‑order functions** (`List.exists`, `Seq.forall`)  
- **Pure functions** for move validation and win logic  

---

## 🧩 Game Architecture

GomokuProject.fsproj
├── models/
│   ├── Board.fs         # Board representation and types
│   └── Menu.fs          # Menu and user input handling
├── Logic/
│   └── Game.fs          # Core game logic, win/draw checks, CPU move
└── src/
└── Program.fs       # Entry point and game loop



---

## 📐 Mathematical & Logical Foundations

### **Gherkin Scenarios (BDD)**
Used to define behaviour from the user’s perspective, mapping to Hoare Logic (Given–When–Then).

### **Set Theory**
- Board positions:  
  \( B = \{(x, y) | 1 \le x, y \le 15\} \)
- Players:  
  \( P = \{Player1, Player2\} \)
- Cell states:  
  \( K = \{Empty, Black, White\} \)

### **Graph Theory**
- Each cell is a node  
- Edges connect horizontal, vertical, and diagonal neighbours  
- Win = path of 5 connected nodes with same owner  

---

## 🧪 Testing

### **Manual Testing**
Covers:
- Valid/invalid moves  
- Win conditions  
- Draw detection  
- CPU behaviour  
- Turn switching  
- Replay options  

### **Automated Testing**
- **xUnit + FsUnit** for unit tests  
- **TickSpec** for BDD behavioural tests  
- Tests include:
  - Player switching  
  - Win logic (all directions)  
  - Draw detection  
  - Behavioural scenarios mapped from Gherkin  

---

## 🤖 CPU Logic

The CPU selects a random valid move from available empty cells.  
Future improvements may include:
- Minimax algorithm  
- Heuristic evaluation  
- Difficulty levels  

---

## 🚀 Future Improvements

- Implement Minimax for smarter CPU  
- Introduce monadic patterns (`Option`, `Result`, `Async`)  
- Add GUI version  
- Improve performance with optimised board traversal  
- Online multiplayer support  

---

## 🛠️ Technologies Used

- **F#**  
- **.NET SDK**  
- **Visual Studio Code**  
- **xUnit**, **FsUnit**, **TickSpec**  
- **Gherkin** for BDD  
- **Hoare Logic** for formal reasoning  

---

## 📚 Authors

- **Abdulrazig I Adam**
- **Robin Petelo** 
- Birmingham City University 
- Module: *Computer Mathematics and Declarative Programming (CMP5361)*  

---

## 📄 License

This project is for academic and educational purposes.

