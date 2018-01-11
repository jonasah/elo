import * as React from 'react'
import { Link } from 'react-router-dom';

interface PlayerStatsLinkProps {
    player: string;
}

export class PlayerStatsLink extends React.Component<PlayerStatsLinkProps, {}> {
    public render() {
        return <Link to={'/playerstats/' + this.props.player}>
            {this.props.player}
        </Link>
    }
}
