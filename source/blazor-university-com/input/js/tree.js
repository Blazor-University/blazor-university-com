
window.addEventListener("load", initialize);

expandedGroups = JSON.parse(sessionStorage.getItem('expandedGroups'));

//Fold or unfold each content based on state before refresh
//And go to same scroll position before refresh
function loadState() {
  const firstMenuGroup = document.querySelector('#left-sidebar .content-menu .menu-group');

  if (!expandedGroups || !expandedGroups.includes(firstMenuGroup.dataset.groupId)) {
    expandedGroups = new Array(firstMenuGroup.dataset.groupId);
  }

  ensureActivePageAndAncestorsAreExpanded();
    
  expandedGroups.forEach((groupId) => {
    const groups = document.querySelectorAll(`[data-group-id="${groupId}"]`);
    const menus = document.querySelectorAll(`[data-doc-id="${groupId}"]`);
    groups?.forEach(group => {
      group.classList.remove('hidden');     
    });
    menus?.forEach(menu => {
      const arrow = menu.querySelector('.menu-arrow')
      arrow?.classList.add('rotate');
    });
  })

  //load offsetTop position if is exist.
  var sidebarScrollPos = sessionStorage.getItem('sidebarScrollPos');
  if (sidebarScrollPos) document.querySelector('#left-sidebar').scrollTop = sidebarScrollPos;
}

function ensureActivePageAndAncestorsAreExpanded() {
  const activeItem = document.querySelector('.sidebar-nav-item.active');
  const activeMenu = activeItem?.closest('.menu');
  const activeDocId = activeMenu?.dataset.docId;
  if (activeDocId) { ensureGroupId(activeDocId) };

  var currentMenu = activeMenu?.closest('.menu-group');
  while (currentMenu)
  {
    const currentMenuId = currentMenu.dataset.groupId;
    ensureGroupId(currentMenuId);
    currentMenu = currentMenu.parentElement.closest('.menu');
  }
}

function setEvents() {
  const mobileMenu = document.querySelector("#mobile-menu");
  const backgroundBackdrop = document.querySelector('#backgroundBackDrop');
  const toggle = document.querySelector("#toggle");
  const slide = document.querySelector("#slide");
  const closeSlider = document.querySelector("#closeSlider");
  const fixedBackground = document.querySelector("#fixedBackground");
  var isBool = true;
  toggle.addEventListener('click', () => {
    if (isBool) {
      mobileMenu.classList.add('translate-x-0');
      mobileMenu.classList.remove('translate-x-full');
      backgroundBackdrop.classList.add('opacity-100');
      backgroundBackdrop.classList.remove('opacity-0');
      backgroundBackdrop.classList.add('bg-gray-500');
      toggle.classList.add('hidden');
      slide.classList.remove('invisible');
      isBool = false;
    } else {
      mobileMenu.classList.remove('translate-x-0');
      mobileMenu.classList.add('translate-x-full');
      backgroundBackdrop.classList.add('opacity-0');
      backgroundBackdrop.classList.remove('opacity-100');
      backgroundBackdrop.classList.remove('bg-gray-500');
      slide.classList.add('invisible');
      isBool = true;
    }
  })
  closeSlider.addEventListener('click', () => {
    mobileMenu.classList.remove('translate-x-0');
    mobileMenu.classList.add('translate-x-full');
    backgroundBackdrop.classList.add('opacity-0');
    backgroundBackdrop.classList.remove('opacity-100');
    backgroundBackdrop.classList.remove('bg-gray-500');
    toggle.classList.remove('hidden');
    slide.classList.add('invisible');
    isBool = true;
  })
}

function setClickEvents() {
  const menuElements = document.querySelectorAll('.sidebar-nav-item');
  menuElements.forEach(element => element.addEventListener('click', menuClick));
  const menuArrowElements = document.querySelectorAll('.menu-arrow');
  menuArrowElements.forEach(element => element.addEventListener('click', menuClick));
}

function menuClick(event) {
  const menu = event.currentTarget.closest('.menu');
  const groupId = menu.dataset.docId;
  const groups = document.querySelectorAll(`[data-group-id="${groupId}"]`);
  const menus = document.querySelectorAll(`[data-doc-id="${groupId}"]`); 
  groups?.forEach(group => {
    group.classList.toggle('hidden');     
  });
  menus?.forEach(menu => {
    const arrow = menu.querySelector('.menu-arrow')
    arrow?.classList.toggle('rotate');
  });
  
  toggleGroupId(groupId);
}

function toggleGroupId(groupId) {
  const index = expandedGroups.indexOf(groupId);
  index === -1 ? expandedGroups.push(groupId) : expandedGroups.splice(index, 1);
  sessionStorage.setItem('expandedGroups', JSON.stringify(expandedGroups));
}

function ensureGroupId(groupId) {
  if (!expandedGroups || !expandedGroups.includes(groupId)) {
    expandedGroups.push(groupId);
  }
}

function setSideBarEvents(){
  const sideBar = document.querySelector('#left-sidebar');
  sideBar.addEventListener('scroll', saveScrollPosition);
}

function saveScrollPosition() {
  sessionStorage.setItem('sidebarScrollPos', document.querySelector('#left-sidebar').scrollTop);
}

function loadHighlightThemeBaseonOperation() {
  if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
    //darkmode
    document.querySelector(`link[title="highlight_dark"]`).removeAttribute("disabled");
    document.querySelector(`link[title="highlight_light"]`).setAttribute("disabled", "disabled");
  } else {
    //lightmode
    document.querySelector(`link[title="highlight_dark"]`).setAttribute("disabled", "disabled");
    document.querySelector(`link[title="highlight_light"]`).removeAttribute("disabled");
  }
}

function initialize() {
  loadState();
  loadHighlightThemeBaseonOperation();
  setClickEvents();
  setSideBarEvents();
  setEvents();
}
