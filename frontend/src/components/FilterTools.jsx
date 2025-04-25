import React from 'react';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import '../styles/filter-tools.css';


function SearchBar({ placeholder, value, onChange }) {
    return (
    <div className="search-bar">
        <FontAwesomeIcon icon={faMagnifyingGlass}/>
        <input
        type="text"
        placeholder={placeholder}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        />
    </div>
    );
}


function SortSelect({ value, onChange, options }) {
    return (
    <div className="sort-select">
        <select value={value} onChange={e => onChange(e.target.value)}>
            {options.map(opt => (
            <option key={opt.value} value={opt.value}>{opt.label}</option>
            ))}
        </select>
    </div>
    );
}


function FilterSelect({ value, onChange, options, placeholder }) {
    return (
    <div className="filter-select">
        <select value={value} onChange={e => onChange(e.target.value)}>
        <option value="">{placeholder}</option>
        {options.map(opt => (
            <option key={`${opt.key}-${opt.value}`} value={opt.value}>{opt.label}</option>
        ))}
        </select>
    </div>
    );
}

export {SearchBar, SortSelect, FilterSelect}; 