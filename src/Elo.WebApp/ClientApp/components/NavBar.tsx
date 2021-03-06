import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';

export class NavBar extends React.Component<{}, {}> {
    public render() {
        return <nav className='navbar navbar-inverse navbar-static-top'>
            <div className='container-fluid'>
                <div className='navbar-header'>
                    <button type='button' className='navbar-toggle collapsed' data-toggle='collapse' data-target='.navbar-collapse'>
                        <span className='sr-only'>Toggle navigation</span>
                        <span className='icon-bar'></span>
                        <span className='icon-bar'></span>
                        <span className='icon-bar'></span>
                    </button>
                    <Link className='navbar-brand' to={ '/' }>Elo</Link>
                </div>
                <div className='navbar-collapse collapse'>
                    <ul className='nav navbar-nav'>
                        <li>
                            <NavLink to={'/'} exact activeClassName='active'>Home</NavLink>
                        </li>
                        <li>
                            <NavLink to={ '/ratings' } activeClassName='active'>Ratings</NavLink>
                        </li>
                        <li>
                            <NavLink to={'/playerstats'} activeClassName='active'>Player Stats</NavLink>
                        </li>
                        <li>
                            <NavLink to={'/games'} activeClassName='active'>Games</NavLink>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>;
    }
}
