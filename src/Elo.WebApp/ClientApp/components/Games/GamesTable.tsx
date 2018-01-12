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
}

interface GamesTableProps {
    pageSize: number;
}

export class GamesTable extends React.Component<GamesTableProps, GamesTableState> {
    constructor(props: GamesTableProps) {
        super(props);

        this.state = { games: [], page: 1 };

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
        fetch('api/elo/games?page=' + this.state.page + '&pageSize=' + this.props.pageSize)
            .then(response => response.json() as Promise<GameDto[]>)
            .then(data => this.setState({ games: data }));
    }
}
