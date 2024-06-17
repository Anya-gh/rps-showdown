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
  playstyle: {
    style: string,
    description: string
  },
  games: number,
  levelID: number
}

export type PlayType = {
  levelChoice: string,
  result: string
}

export type SpectateType = {
  playerChoice: string,
  levelChoice: string,
  result: string
}
