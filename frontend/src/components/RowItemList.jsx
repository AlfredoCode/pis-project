import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/row-item-list.css';

export default function RowItemList({ title, items }) {
    return (
    <div className="row-item-list">
        <h3>{title}</h3>
        <div className="row-list">
        {items.map(item => (
            <Link className="row-item" to={item.link} >
                {item.label}
            </Link>
        ))}
        </div>
    </div>
    );
}