import * as React from 'react'
import { RouteComponentProps, withRouter } from 'react-router';
import { Head2HeadRecords } from './Head2HeadRecords';

interface PlayerStatsPageProps {
    player: string;
}

export class PlayerStatsPage extends React.Component<RouteComponentProps<PlayerStatsPageProps>, {}> {
    player: string;

    constructor(props: RouteComponentProps<PlayerStatsPageProps>) {
        super(props);

        this.player = this.props.match.params.player;
    }

    public render() {
        return <div>
            <div className="page-header">
                <h1>Player Stats - {this.player}</h1>
            </div>
            <Head2HeadRecords player={this.player} />
        </div>;
    }
}
