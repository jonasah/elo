import * as React from 'react'
import { Link } from 'react-router-dom';
import { PlayerStatsLink } from '../Common/PlayerStatsLink';

interface GameDto {
    id: number;
    winner: string;
    loser: string;
    date: string;
}

interface GamesTableState {
    games: GameDto[];
    page: number;
    pageSize: number;
}

export class GamesTable extends React.Component<{}, GamesTableState> {
    constructor() {
        super();

        this.state = { games: [], page: 1, pageSize: 25 };

        this.fetchGames();
    }

    public render() {
        return <div>
            <table className="table table-condensed table-striped table-hover table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Winner</th>
                        <th className="text-center">Loser</th>
                        <th className="text-center">Date</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.games.map(game =>
                        <tr key={game.id}>
                            <td className="text-center">
                                <PlayerStatsLink player={game.winner}/>
                            </td>
                            <td className="text-center">
                                <PlayerStatsLink player={game.loser}/>
                            </td>
                            <td className="text-center">{game.date}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;
    }

    fetchGames() {
        fetch('api/elo/games?page=' + this.state.page + '&pageSize=' + this.state.pageSize)
            .then(response => response.json() as Promise<GameDto[]>)
            .then(data => this.setState({ games: data }));
    }
}
