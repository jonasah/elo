import * as React from 'react'
import { Link } from 'react-router-dom'
import { PlayerStatsLink } from '../Common/PlayerStatsLink';
import * as Api from '../../api';

interface RatingsTableState {
    ratings: Api.Models.Rating[];
}

export class RatingsTable extends React.Component<{}, RatingsTableState> {
    timerId: number;

    constructor() {
        super();
        this.state = { ratings: [] };
    }

    public render() {
        return <table className="table table-condensed table-striped table-hover table-bordered">
            <thead>
                <tr>
                    <th className="text-center">Rank</th>
                    <th className="text-center">Player</th>
                    <th className="text-center">Rating</th>
                    <th className="text-center">Games</th>
                    <th className="text-center">Wins</th>
                    <th className="text-center">Losses</th>
                    <th className="text-center">Pct</th>
                    <th className="text-center"><abbr title="Current streak">Stk</abbr></th>
                </tr>
            </thead>
            <tbody>
                {this.state.ratings.map(rating =>
                    <tr key={rating.id}>
                        <td className="text-center">{rating.rank}</td>
                        <td className="text-center">
                            <PlayerStatsLink player={rating.player}/>
                        </td>
                        <td className="text-center">{rating.rating}</td>
                        <td className="text-center">{rating.gamesPlayed}</td>
                        <td className="text-center">{rating.wins}</td>
                        <td className="text-center">{rating.losses}</td>
                        <td className="text-center">{(100 * rating.pct).toFixed(1)}</td>
                        <td className="text-center">{this.getStreakString(rating.streak)}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }

    fetchRatings() {
        Api.getRatings()
            .then(data => {
                this.setState({ ratings: data });
            });
    }

    getStreakString(streak: number) {
        // positive: winning streak, negative: losing streak
        return (streak > 0 ? `W${streak}` : `L${-streak}`);
    }

    componentWillMount() {
        this.fetchRatings();
    }

    componentDidMount() {
        this.timerId = setInterval(() => this.fetchRatings(), 3000);
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }
}
