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

export type PlayType = {
  botChoice: string,
  result: string
}

export type SpectateType = {
  playerChoice: string,
  levelChoice: string,
  result: string
}
