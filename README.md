# 🎮 Player Wallet Simulator

A demo application that simulates the operations of a player wallet in an online casino.   

The application is built using **.NET 9** and includes:
- **Console Application**  
- **Unit Tests Project** (for testing core functionalities)  

## 🔧 Configuration

The application supports **dynamic configuration** through `appsettings.json`.  
```json
"SlotGameConfig": {
  "MinBet": "Defines the minimum bet amount",
  "MaxBet": "Defines the maximum bet amount",
  "LoseProbability": "Defines the percentage of bets that should result in a loss",
  "WinProbability": "Defines the percentage of bets that should result in a regular win",
  "BigWinProbability": "Defines the percentage of bets that should result in a big win",
  "BigWinMinMultiplier": "Defines the minimum multiplier for a big win",
  "BigWinMaxMultiplier": "Defines the maximum multiplier for a big win"
}
```

You can modify these settings in **`src/PlayerWalletSimulator.Console/appsettings.json`** to customize game behavior.

## 🚀 Running the Application

### 1️ Clone the Repository
```sh
git clone https://github.com/milenyorgov/player-wallet-simulator.git
```

### 2️ Run in an IDE  
Open the project in your favorite IDE (**Visual Studio**, **Rider**, etc.) and run the **console application**.


### 3️ Run via PowerShell Script  
The project includes a **PowerShell script** (`build-and-run.ps1`) that:
- **Builds** the project
- **Runs tests**
- **Starts** the application

#### **Run the script:**
```powershell
cd [project location]
./build-and-run.ps1
```

### 4 Run with Docker 

The project includes a **Dockerfile** for containerized execution.


### Build the Docker Image
```sh
cd [project location]/src/PlayerWalletSimulator.Console/
docker build -t player-wallet-simulator .
```

### Run the Container
```sh
docker run --env-file .env -it player-wallet-simulator
```
**Note:** If you want to change the default settings, modify **`[project location]/src/PlayerWalletSimulator.Console/.env`** before running the container.
