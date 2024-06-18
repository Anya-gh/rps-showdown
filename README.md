# rps-showdown

 A simulator for rock paper scissors.

# Running the app

I've provided a `start.sh` script to run both the frontend and backend. Run it by using the command `./start.sh` in command line in the top-level directory. Please make sure nothing is running on ports 3000 and 5000; the ports both the frontend and backend are running on are shown in the console. If it doesn't run, make sure the permissions are set correctly: use `chmod +x start.sh` If they aren't correct please stop anything running on ports 3000 and 5000 and try again.

If the script does not work, try `npm run dev` for the frontend, and `dotnet run` for the backend. If there's an issue with packages, install frontend packages with `npm install` and backend packages with `dotnet restore`. If it still does not work, please contact me; I've tested it as much as I can on my system so hopefully the issue is not with the app.

# Testing the app

Likewise, I've supplied a `test.sh` script. Again, please run it from the command line with `./test.sh` from the top-level directory.

# Backend

The backend is built in .NET, using the Entity Framework Core for the database, which is implemented using an sqlite server. Unit testing is in Xunit.

# Frontend

The frontend is build in React with Typescript. The CSS is done using Tailwind. The design is responsive, so please try using a mobile view to try it out.

# CORS issue

If you try to login and nothing happens, or any other buttons stop working, please check the browser console. It may be a CORS issue. I have implemented a CORS policy so this should not happen, but it has happened several times on my Firefox browser and seems to fix itself every time. If it happens, please try another window, tab, browser or machine.

# GitHub folder name desync

For some reason GitHub isn't recognising that the path to a file changes when a folder name changes, at least for me at the moment. I believe I have fixed the issue, but if the app throws an error related to the `Home` component, please try renaming the `./frontend/src/Home` folder that the `Home` component to something else, saving, and then renaming it back. If it still doesn't work please let me know. Unfortunately this is not something I can easily control as it works on my end which is synced with the GitHub repository.

# Database

The database is empty. If you want to reset it after using it, you can:
- Delete RPSDb.db
- Create RPSDb.db
- Delete the folder `./backend/Migrations`
- Run `dotnet ef migrations add InitialCreate`
- Run `dotnet ef database update`

# Bot levels:
There are three bot levels:
- Beginner
- Intermediate
- Advanced

If you inspect the `./backend/Handlers/BotHandler` file, you'll be able to see how each is implemented. Below is a general overview of each. Note: you cannot spectate a bot playing against itself. The main reason is because, with the way I have the database set up, it would require each bot to be registered as a user. The reason I chose not to accommodate for this is because spectating a bot playing against itself won't be very interesting: it will either by cyclic or both will pick the same option. It will also give very obvious insight into how they work, making them very easy to beat.

## Difficulty in RPS

Before going into detail on each bot, first let's have a look at what makes a 'good' strategy in RPS. The answer is very simple: playing each option with equal probability. Actually playing each with equal probability is an infallible strategy. In game theory, RPS can be analysed as a normal-form zero-sum two-player game. Doing so will show that when playing against an opponent playing each option with 1/3, you can do no better than win, draw and lose 33% of the time.

Humans can't do this well. Statistically, humans are not very good at truly being random. Computers are, on the other hand. However, just picking each option randomly isn't interesting. The bots I've created focus on interesting strategies, that try to punish the player and maximise their own winnings, than settling for the 33% win, draw, lose distribution aforementioned.

## Beginner

The beginner bot will always play whatever one last time. This is a fairly ineffective strategy, as no option is better than another inherently, and as it's fairly easy to see through.

## Intermediate

The intermediate bot follows the strategy discussed in this [paper](https://arxiv.org/pdf/1404.5199v1). Essentially, they found that if someone won, they would try to play the same thing again since it won last time, and if they lost they'll play whatever beats what they just lost to. For example, if player 1 plays rock and loses to player 2 playing paper, player 1 will play scissors and player 2 will continue playing paper (generally). The intermediate bot assumes you will follow this pattern, and acts accordingly to beat it. This comes down to:
- If it wins, it plays whatever it's opponent just lost with
- If it loses, it plays whatever beats the option it just lost to

## Advanced

The advanced bot uses a Markov chain. This is a fundamental system in machine learning (reinforcement learning, specifically). Essentially, it models the state and all the possible transitions, and learns which transition you are most likely to take. You can model the state and transition how you wish, but the most natural way, I believe, is to model the previous match as the state and and the next round as possible transitions.

To do so it constructs a transition matrix, which looks like this:

  [(R->R), (R->P), (R->S),\
   (P->R), (P->P), (P->S),\
   (S->R), (S->P), (S->S)]

where X->Y is the probability of choosing Y after just choosing X. To get the probability of X->Y, it calculates: (no. of times X->Y / no. of times X). Then, if we now take X to be the player's previous choice, it finds what the player is most likely to do by picking the Y with the greatest X->Y probability; the choice they're most likely to pick given they just picked X.

## Counterfactual Regret Minimization (CFRM)

One of the better algorithms in RPS is known as Counterfactual Regret Minimization, or just regret matching. This is a simple algorithm to learn the opponent's choice distribution. The algorithm is simple: keep track of the regret for each option. The algorithm 'regrets' an option if it would have won last time. We can model this by giving an option a value of 1 if it would have won, 0 if it would have drawn, and -1 if it lost. For example, if the algorithm played rock and just lost to paper, scissors gets a regret of 1, rock gets a regret of 0 and paper gets a regret of -1. To calculate the probability of choosing an option in the next round, it sums the total regret (or 0 if the sum is negative) for each option over the total number of rounds. Options with negative regret will continue to become less and less likely, and options with positive regret will become more and more likely.

Indeed, it can be shown that two agents using regret matching will converge to a correlated equilibrium: that is to say, both will end up playing every option with 33% probability with no desire to change their probabilities (once they've converged).

I didn't implement this algorithm because it essentially achieves the same result as the Markov chain, but in a different way; I just wanted to mention it since it's another interesting strategy.



