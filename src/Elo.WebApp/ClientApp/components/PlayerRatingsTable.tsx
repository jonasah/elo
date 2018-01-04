import * as React from 'react'

interface PlayerRatingDto {
    id: number;
    rank: number;
    player: string;
    rating: number;
    gamesPlayed: number;
    wins: number;
    losses: number;
}

interface PlayerRatingsTableState {
    ratings: PlayerRatingDto[];
}

export class PlayerRatingsTable extends React.Component<{}, PlayerRatingsTableState> {
    timerId: number;

    constructor() {
        super();
        this.state = { ratings: [] };
    }

    public render() {
        return <table className="table">
            <thead>
                <tr>
                    <th>Rank</th>
                    <th>Player</th>
                    <th>Rating</th>
                    <th>Games played</th>
                    <th>Wins</th>
                    <th>Losses</th>
                </tr>
            </thead>
            <tbody>
                {this.state.ratings.map(rating =>
                    <tr key={rating.id}>
                        <td>{rating.rank}</td>
                        <td>{rating.player}</td>
                        <td>{rating.rating}</td>
                        <td>{rating.gamesPlayed}</td>
                        <td>{rating.wins}</td>
                        <td>{rating.losses}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }

    fetchRatings() {
        fetch('api/elo/ratings')
            .then(response => response.json() as Promise<PlayerRatingDto[]>)
            .then(data => {
                this.setState({ ratings: data });
            });
    }

    componentDidMount() {
        this.timerId = setInterval(() => this.fetchRatings(), 1000);
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }
}
