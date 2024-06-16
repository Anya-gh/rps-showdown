export type StatsType = {
  winRate: number,
  longestStreak: number,
  choiceDistribution: {
    rock: number,
    paper: number,
    scissors: number
  },
  ace: string,
  nemesis: string,
  style: string,
  levelID: number
}