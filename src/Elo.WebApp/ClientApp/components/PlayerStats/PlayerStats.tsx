import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Head2HeadRecords } from './Head2HeadRecords';
import { LatestGames } from '../Games/LatestGames';
import { ExpectedScores } from './ExpectedScores';
import { SeasonSelect } from '../Seasons/SeasonSelect';

interface PlayerStatsProps {
    player: string;
}

interface PlayerStatsState {
    selectedSeason: string;
}

export class PlayerStats extends React.Component<RouteComponentProps<PlayerStatsProps>, PlayerStatsState> {
    constructor(props: RouteComponentProps<PlayerStatsProps>) {
        super(props);

        this.state = { selectedSeason: '' };

        this.onSeasonSelected = this.onSeasonSelected.bind(this);
    }

    public render() {
        return <div>
            <div className="page-header">
                <h1>{this.props.match.params.player} <small>Player Stats</small></h1>
                <SeasonSelect
                    selectedSeason={this.state.selectedSeason}
                    onSeasonSelected={this.onSeasonSelected}
                    player={this.props.match.params.player}
                />
            </div>
            <div className="row">
                <div className="col-sm-6">
                    <Head2HeadRecords player={this.props.match.params.player} season={this.state.selectedSeason} />
                    <hr />
                    <LatestGames
                        player={this.props.match.params.player}
                        numGames={10}
                        showActions={false}
                        headerSize={2}
                    />
                </div>
                <div className="col-sm-6">
                    <ExpectedScores player={this.props.match.params.player} season={this.state.selectedSeason} />
                </div>
            </div>
        </div>;
    }

    onSeasonSelected(season: string) {
        this.setState({ selectedSeason: season });
    }
}
