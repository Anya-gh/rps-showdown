import { StatsType } from "../Types";
import Card from "./Card";
import winRateIcon from "../assets/win-rate.png"
import streakIcon from "../assets/streak.png"

type PerformanceProps = {
  userStats: StatsType
}

function Performance({ userStats } : PerformanceProps) {

  return (
    <div className="mt-5 w-full flex flex-col md:flex-row md:justify-evenly md:w-2/3 items-center">
      <Card>
        <div className="flex flex-col items-center font-bold justify-center h-full">
          <span className="flex flex-row items-center mb-auto">
            <img className="w-6 h-6 mr-3" src={winRateIcon} />
            <h1 className="text-lg">Win Rate</h1>
          </span>
          <span className="mb-auto">
            <p className="text-5xl text-center mb-2">{Math.round(userStats.winRate*100)} %</p>
            <p className="text-sm italic font-thin text-center">in {userStats.games} games (excluding draws)</p>
          </span>
        </div>
      </Card>
      <Card>
        <div className="flex flex-col items-center font-bold justify-center h-full">
          <span className="flex flex-row items-center mb-auto">
            <img className="w-6 h-6 mr-3" src={streakIcon} />
            <h1 className="text-lg">Longest Win Streak</h1>
          </span>
          <p className="text-5xl mb-auto">{userStats.longestStreak} Wins</p>
        </div>
      </Card>
    </div>
  )
}

export default Performance;
