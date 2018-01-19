import * as React from 'react';
import { Link } from 'react-router-dom';
import { PlayerStatsLink } from '../Common/PlayerStatsLink';
import * as Api from '../../api';

interface Head2HeadRecordsProps {
    player: string;
}

interface Head2HeadRecordsState {
    records: Api.Models.Head2HeadRecord[];
}

export class Head2HeadRecords extends React.Component<Head2HeadRecordsProps, Head2HeadRecordsState> {
    constructor(props: Head2HeadRecordsProps) {
        super(props);
        this.state = { records: [] };

        Api.getHead2HeadRecords(this.props.player)
            .then(data => this.setState({ records: data }));
    }

    public render() {
        return <div>
            <h2>Head 2 Head Records</h2>
            <table className="table table-condensed table-striped table-hover table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Opponent</th>
                        <th className="text-center">Games</th>
                        <th className="text-center">Wins</th>
                        <th className="text-center">Losses</th>
                        <th className="text-center">Pct</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.records.map(h2h =>
                        <tr key={h2h.opponent}>
                            <td className="text-center">
                                <PlayerStatsLink player={h2h.opponent}/>
                            </td>
                            <td className="text-center">{h2h.gamesPlayed}</td>
                            <td className="text-center">{h2h.wins}</td>
                            <td className="text-center">{h2h.losses}</td>
                            <td className="text-center">{(100 * h2h.pct).toFixed(1)}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;
    }
}