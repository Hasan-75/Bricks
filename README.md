# 🧱 Bricks
*Yet another unnecessary console game*

---

### 💡 Inspiration
After watching the *[Tetris (2023)](https://www.imdb.com/title/tt12758060/)* movie, I was fascinated by the story of how Alexey Pajitnov, a Russian programmer, created Tetris in 1984.
He built the game on a computer without a graphics card, relying entirely on keyboard characters like square brackets for visuals.

This motivated me to take an attempt and build this console version of the **[Bricks](https://dosgames.com/game/break-machine/)** game, which I used to play in my childhood.

---
<details>
<summary><strong>🧩 Here's how Pajitnov drew Tetromino Shapes using <code>[ ]</code></strong></summary>

```
I-shape:        O-shape:        T-shape:

[ ][ ][ ][ ]    [ ][ ]          [ ][ ][ ]
                                [ ]

S-shape:        Z-shape:        J-shape:

  [ ][ ]        [ ][ ]            [ ]
[ ][ ]            [ ][ ]          [ ][ ]

L-shape:

  [ ]
  [ ]
[ ][ ]
```

</details>

### 🕹️ Video Demo
![Bricks Demo](./Demo/Bricks.gif)
---
### ⚙️ Installation & Setup

Follow these steps to get **Bricks** running on your machine.

#### 📋 Prerequisites

- 📦 [.NET SDK](https://dotnet.microsoft.com/download) (**.NET 8.0** or higher recommended)
- 💻 A code editor or IDE that supports C# (Visual Studio, Visual Studio Code, Rider, etc.)  
- 🖥️ Command-line / terminal access  

#### 🚀 Steps

1. **📂 Clone the repository**

    ```bash
    git clone https://github.com/Hasan-75/Bricks.git
    cd Bricks
    ```

2. **🔄 Restore dependencies**

    ```bash
    dotnet restore
    ```

3. **🛠️ Build the project**

    ```bash
    dotnet build
    ```

4. **🎮 Run the game**

    From the solution or project folder:

    ```bash
    dotnet run --project Bricks
    ```

    Or if you're inside the project directory (where the `.csproj` is):

    ```bash
    dotnet run
    ```



### 🎮 How to Play

- Use **Left Arrow (←)** and **Right Arrow (→)** keys to move the paddle left or right.  
- The **ball** will bounce off the paddle and hit the bricks.  
- **Do not let the ball fall** below the paddle. You have limited lives.  
- **Break all the bricks** to win the game.  
- Press **Q** at any time to quit the game.
