import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { AddGameForm } from '../Games/AddGameForm';
import { Ratings } from '../Ratings/Ratings';
import { LatestGames } from '../Games/LatestGames';

export class HomePage extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <div className="row">
                <div className="col-sm-12">
                    <AddGameForm />
                    <hr/>
                </div>
            </div>
            <div className="row">
                <div className="col-sm-8 col-md-9">
                    <Ratings headerSize={2} onlyActiveSeasons={true} enablePlayerFilter={true} />
                </div>
                <div className="col-sm-4 col-md-3">
                    <LatestGames
                        numGames={10}
                        showDate={false}
                        showActions={false}
                        headerSize={2}
                    />
                </div>
            </div>
        </div>;
    }
}
