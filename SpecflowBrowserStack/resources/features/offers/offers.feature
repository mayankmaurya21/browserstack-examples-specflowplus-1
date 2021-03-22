@Mobile
Feature: Offers Feature

  Scenario: Offers for mumbai geo-location
    Given I navigate to website on mobile.
    Then I click on Sign-In link
    When I type "fav_user" in username field
    When I type "testingisfun99" in password field
    Then I press Log-In Button
    Then I click on Offers link
    Then I should see Offer elements
