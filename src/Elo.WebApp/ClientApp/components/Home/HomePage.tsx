import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { AddGameForm } from '../Ratings/AddGameForm';
import { RatingsTable } from '../Ratings/RatingsTable';
import { LatestGamesTable } from '../Games/LatestGamesTable';

export class HomePage extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <div className="row">
                <div className="col-sm-12">
                    <h2>Add Game</h2>
                    <AddGameForm />
                    <hr/>
                </div>
            </div>
            <div className="row">
                <div className="col-sm-8 col-md-9">
                    <h2>Ratings</h2>
                    <RatingsTable />
                </div>
                <div className="col-sm-4 col-md-3">
                    <h2 className="text-center">Latest Games</h2>
                    <LatestGamesTable numGames={10} showDate={false} />
                </div>
            </div>
        </div>;
    }
}
