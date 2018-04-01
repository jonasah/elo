import * as React from 'react';
import { PlayerStatsLink } from '../Common/PlayerStatsLink';
import * as Api from '../../api';

interface ExpectedScoresProps {
    player: string;
    season: string;
}

interface ExpectedScoresState {
    scores: Api.Models.ExpectedScore[];
}

export class ExpectedScores extends React.Component<ExpectedScoresProps, ExpectedScoresState> {
    constructor(props: ExpectedScoresProps) {
        super(props);

        this.state = { scores: [] };
    }

    public render() {
        return <div>
            <h2>Expected Scores</h2>
            <table className="table table-condensed table-striped table-hover table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Opponent</th>
                        <th className="text-center">Score</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.scores.map(score =>
                        <tr key={score.opponent}>
                            <td className="text-center">
                                <PlayerStatsLink player={score.opponent} />
                            </td>
                            <td className="text-center">{score.score.toFixed(2)}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;
    }

    fetchScores(props: ExpectedScoresProps) {
        if (props.season === undefined) {
            return;
        }

        Api.getExpectedScores(props.player, props.season).then(data => this.setState({ scores: data }));
    }

    componentWillMount() {
        this.fetchScores(this.props);
    }

    componentWillReceiveProps(nextProps: Readonly<ExpectedScoresProps>) {
        if (this.props.player != nextProps.player || this.props.season != nextProps.season) {
            this.setState({ scores: [] });
            this.fetchScores(nextProps);
        }
    }
}
